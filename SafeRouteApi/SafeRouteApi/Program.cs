namespace SafeRouteApi
{
    using Microsoft.EntityFrameworkCore;
    using SafeRouteApi.Data;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Register DbContext using Azure Service Connector/AppSettings connection string
            // Expected key set by Service Connector/App Service: ConnectionStrings:DatabaseConnection
            var dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection");
            if (!string.IsNullOrWhiteSpace(dbConnection))
            {
                builder.Services.AddDbContext<SafeRouteDbContext>(options =>
                    options.UseSqlServer(dbConnection));
            }
            else
            {
                // No connection string found; DbContext won't be registered. Provide a simple console hint for local dev.
                Console.WriteLine("Warning: No ConnectionStrings:DatabaseConnection found. EF DbContext not registered.");
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Simple DB health endpoint to verify connectivity
            app.MapGet("/db/health", async (SafeRouteDbContext? db, CancellationToken ct) =>
            {
                if (db is null)
                {
                    return Results.Problem("DbContext not registered. Ensure ConnectionStrings:DatabaseConnection is set.", statusCode: 500);
                }

                try
                {
                    var canConnect = await db.Database.CanConnectAsync(ct);
                    return canConnect
                        ? Results.Ok(new { status = "ok" })
                        : Results.Problem("Cannot connect to database.", statusCode: 500);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Connection failed: {ex.Message}", statusCode: 500);
                }
            })
            .WithName("DbHealth")
            .WithOpenApi();

            app.MapControllers();

            app.Run();
        }
    }
}
