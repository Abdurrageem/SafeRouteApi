using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        // DbContext
        var conn = builder.Configuration.GetConnectionString("SafeRouteDb");
        builder.Services.AddDbContext<SafeRouteDbContext>(options => options.UseSqlServer(conn));

        // AutoMapper profile assembly
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddMemoryCache();

        // CORS
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(o => o.AddPolicy("DefaultCors", p => p.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

        // JWT secret from env override
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

        // Services
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        builder.Services.AddScoped<IDriverService, DriverService>();
        builder.Services.AddScoped<IRouteService, RouteService>();

        // Middleware
        builder.Services.AddTransient<ExceptionHandlingMiddleware>();

        // Seed
        builder.Services.AddTransient<SeedData>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Run seed at startup
        using (var scope = app.Services.CreateScope())
        {
            var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
            await seeder.InitializeAsync();
        }

        app.UseHttpsRedirection();
        app.UseCors("DefaultCors");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();
        app.Run();
    }
}

// Token service & password hasher
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

// Global exception handling middleware
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
