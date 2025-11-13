using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SafeRoute.Data;
using System.Text;
using AutoMapper;
using SafeRoute.Services;
using SafeRoute.Services.Interfaces;

namespace SafeRoute;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApi();

        // Always use fresh SQLite file for local dev (regenerates schema each run if flag set)
        var sqliteConn = builder.Configuration.GetConnectionString("SafeRouteSqlite") ?? "Data Source=saferoute.db";
        var rebuild = builder.Configuration.GetValue<bool>("RebuildSqlite");

        if (rebuild && File.Exists("saferoute.db"))
        {
            File.Delete("saferoute.db");
        }

        builder.Services.AddDbContext<SafeRouteDbContext>(options =>
        {
            options.UseSqlite(sqliteConn);
        });

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
        builder.Services.AddTransient<SeedData>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeRoute API v1");
                c.RoutePrefix = "swagger";
            });
            app.MapOpenApi();
        }

        // Build brand new schema then seed
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SafeRouteDbContext>();
            await db.Database.EnsureDeletedAsync(); // ensure no stale schema
            await db.Database.EnsureCreatedAsync(); // create from current model
            var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
            await seeder.InitializeAsync();
        }

        app.UseHttpsRedirection();
        app.UseCors("DefaultCors");
        app.UseAuthentication();
        app.UseAuthorization();
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
