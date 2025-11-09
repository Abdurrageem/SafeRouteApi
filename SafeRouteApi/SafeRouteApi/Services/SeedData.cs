using Microsoft.EntityFrameworkCore;
using SafeRouteApi.Data;
using SafeRouteApi.Models;

namespace SafeRouteApi.Services;

public class SeedData
{
    private readonly SafeRouteDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly ILogger<SeedData> _logger;

    public SeedData(SafeRouteDbContext db, IPasswordHasher hasher, ILogger<SeedData> logger)
    { _db = db; _hasher = hasher; _logger = logger; }

    public async Task InitializeAsync()
    {
        // apply migrations
        await _db.Database.MigrateAsync();

        if (!await _db.Companies.AnyAsync())
        {
            var company = new Companies
            {
                Name = "SafeRoute Co",
                Address = "",
                CompanyRegistration = "SR-0001",
                Email = "info@saferoute.local",
                ContactNumber = "000"
            };
            _db.Companies.Add(company);
            await _db.SaveChangesAsync();
        }

        var comp = await _db.Companies.FirstAsync();

        // Admin user (optional)
        if (!await _db.Users.AnyAsync(u => u.Email == "admin@saferoute.local"))
        {
            var admin = new Users
            {
                CompanyId = comp.CompanyId,
                Email = "admin@saferoute.local",
                Password = _hasher.Hash("Admin@12345"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
            _db.Users.Add(admin);
        }

        // Sample driver and user
        if (!await _db.Users.AnyAsync(u => u.Email == "driver@saferoute.local"))
        {
            var u = new Users
            {
                CompanyId = comp.CompanyId,
                Email = "driver@saferoute.local",
                Password = _hasher.Hash("Driver@12345"),
                Role = "Driver",
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
            _db.Users.Add(u);
            await _db.SaveChangesAsync();

            var d = new Drivers
            {
                UserId = u.UserId,
                Name = "John",
                Surname = "Mbeki",
                LicenseNumber = "CA-123-456",
                VehicleRegistration = "CAW 12345",
                VehicleModel = "Toyota Hilux"
            };
            _db.Drivers.Add(d);
        }

        // Seed a couple of risk zones
        if (!await _db.RiskZones.AnyAsync())
        {
            _db.RiskZones.AddRange(
                new RiskZones { Name = "Nyanga", Latitude = -34.0276f, Longitude = 18.5881f, RiskLevel = "High", Incident = false, IncidentCount = 0 },
                new RiskZones { Name = "Khayelitsha", Latitude = -34.0290f, Longitude = 18.6790f, RiskLevel = "High", Incident = false, IncidentCount = 0 }
            );
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Database seeded");
    }
}
