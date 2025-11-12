using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using SafeRouteApi.Data;
using System.Text;
using AutoMapper;
using SafeRouteApi.Services;
using SafeRouteApi.Services.Interfaces;

namespace SafeRouteApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Local URLs (match launchSettings)
        builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApi();

        // Choose DB provider: prefer SQLite in Development if configured
        var useSqlite = builder.Configuration.GetValue<bool>("UseSqlite");
        if (useSqlite)
        {
            var sqlite = builder.Configuration.GetConnectionString("SafeRouteSqlite") ?? "Data Source=saferoute.db";
            builder.Services.AddDbContext<SafeRouteDbContext>(options =>
            {
                options.UseSqlite(sqlite);
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
        }
        else
        {
            var conn = builder.Configuration.GetConnectionString("SafeRouteDb");
            if (string.IsNullOrWhiteSpace(conn))
            {
                // Prefer the SafeRouteDB LocalDB instance for local development (Option 1)
                conn = "Server=(localdb)\\SafeRouteDB;Database=SafeRouteApiDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
            }
            builder.Services.AddDbContext<SafeRouteDbContext>(options =>
            {
                options.UseSqlServer(conn);
                // Suppress pending model changes warning (treat as log only during dev seeding)
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
        }

        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddMemoryCache();

        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(o => o.AddPolicy("DefaultCors", p => p.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

        var jwtSection = builder.Configuration.GetSection("Jwt");
        var envKey = Environment.GetEnvironmentVariable("SAFEROUTE_JWT_KEY");
        var key = Encoding.UTF8.GetBytes(envKey ?? jwtSection["Key"] ?? "dev-key");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        builder.Services.AddScoped<IDriverService, DriverService>();
        builder.Services.AddScoped<IRouteService, RouteService>();

        // Removed registration of conventional middleware to avoid resolving RequestDelegate via DI
        // builder.Services.AddTransient<ExceptionHandlingMiddleware>();
        builder.Services.AddTransient<SeedData>();

        WebApplication app;
        try
        {
            app = builder.Build();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Host build failed: " + ex); throw; // surface root cause
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeRoute API v1");
                c.RoutePrefix = "swagger"; // UI at /swagger
            });
            app.MapOpenApi();
        }

        // seed wrapped to log inner exceptions explicitly
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
                await seeder.InitializeAsync();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Seeding failed");
                throw; // fail fast so issue is visible
            }
        }

        app.UseHttpsRedirection();
        app.UseCors("DefaultCors");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();
        await app.RunAsync();
    }
}

public interface ITokenService { string CreateToken(int userId, string role, string email); }
public class TokenService : ITokenService
{
    private readonly IConfiguration _config; public TokenService(IConfiguration c) => _config = c;
    public string CreateToken(int userId, string role, string email)
    {
        var jwt = _config.GetSection("Jwt");
        var envKey = Environment.GetEnvironmentVariable("SAFEROUTE_JWT_KEY");
        var keyBytes = Encoding.UTF8.GetBytes(envKey ?? jwt["Key"]!);
        var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
        var claims = new List<System.Security.Claims.Claim>
        {
            new("sub", userId.ToString()),
            new("role", role),
            new("email", email)
        };
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);
        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}
public interface IPasswordHasher { string Hash(string input); bool Verify(string hash, string input); }
public class BcryptPasswordHasher : IPasswordHasher
{ public string Hash(string input) => BCrypt.Net.BCrypt.HashPassword(input); public bool Verify(string hash, string input) => BCrypt.Net.BCrypt.Verify(input, hash); }

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next; private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger){ _next = next; _logger = logger; }
    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "ServerError", message = ex.Message });
        }
    }
}
