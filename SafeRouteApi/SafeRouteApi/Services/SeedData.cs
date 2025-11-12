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

        // Avoid reseeding if substantial data already exists
        if (await _db.Users.CountAsync() > 25) return;

        // Company
        var company = await _db.Companies.FirstOrDefaultAsync();
        if (company == null)
        {
            company = new Companies
            {
                Name = "SafeRoute Co",
                Address = "1 Safety Road, Cape Town",
                CompanyRegistration = "SR-0001",
                Email = "info@saferoute.local",
                ContactNumber = "+27000000000"
            };
            _db.Companies.Add(company);
            await _db.SaveChangesAsync();
        }

        // Users (Admin, DispatcherUser, DriverUser)
        var adminUser = await EnsureUser(company.CompanyId, "admin@saferoute.local", "Admin", "Admin@12345");
        var dispatcherUser = await EnsureUser(company.CompanyId, "dispatcher@saferoute.local", "Dispatcher", "Dispatch@12345");
        var driverUser = await EnsureUser(company.CompanyId, "driver@saferoute.local", "Driver", "Driver@12345");

        // Dispatcher
        Dispatchers dispatcher = await _db.Dispatchers.FirstOrDefaultAsync(d => d.UserId == dispatcherUser.UserId) ?? new Dispatchers
        {
            UserId = dispatcherUser.UserId,
            ShiftStartTime = DateTime.UtcNow.Date.AddHours(6),
            ShiftPattern = "Day",
            IsOnDuty = true
        };
        if (dispatcher.DispatcherId == 0) _db.Dispatchers.Add(dispatcher);
        await _db.SaveChangesAsync();

        // Driver
        Drivers driver = await _db.Drivers.FirstOrDefaultAsync(d => d.UserId == driverUser.UserId) ?? new Drivers
        {
            UserId = driverUser.UserId,
            DispatcherId = dispatcher.DispatcherId,
            Name = "John",
            Surname = "Mbeki",
            LicenseNumber = "CA-123-456",
            VehicleRegistration = "CAW12345",
            VehicleModel = "Toyota Hilux",
            PackageAmount = 42
        };
        if (driver.DriverId == 0) _db.Drivers.Add(driver);
        await _db.SaveChangesAsync();

        // Emergency Contacts
        if (!await _db.EmergencyContacts.AnyAsync())
        {
            _db.EmergencyContacts.AddRange(
                new EmergencyContacts { DriverId = driver.DriverId, Name = "Mary", Surname = "Mbeki", Relationship = "Spouse", PhoneNumber = "+27821234567", Email = "mary.mbeki@example.com" },
                new EmergencyContacts { DriverId = driver.DriverId, Name = "David", Surname = "Mbeki", Relationship = "Brother", PhoneNumber = "+27829876543", Email = "david.mbeki@example.com" }
            );
        }

        // Risk Zones
        if (!await _db.RiskZones.AnyAsync())
        {
            _db.RiskZones.AddRange(
                new RiskZones { Name = "Nyanga", Latitude = -34.0276f, Longitude = 18.5881f, RiskLevel = "High", Incident = false, IncidentCount = 0 },
                new RiskZones { Name = "Khayelitsha", Latitude = -34.0290f, Longitude = 18.6790f, RiskLevel = "High", Incident = false, IncidentCount = 0 },
                new RiskZones { Name = "CBD", Latitude = -33.9249f, Longitude = 18.4241f, RiskLevel = "Medium", Incident = false, IncidentCount = 0 }
            );
        }
        await _db.SaveChangesAsync();

        var zone = await _db.RiskZones.FirstAsync();

        // Routes
        if (!await _db.Routes.AnyAsync())
        {
            var r1 = new Routes
            {
                DriverId = driver.DriverId,
                Start = "Depot A",
                End = "Client B",
                StartTime = DateTime.UtcNow.AddHours(-3),
                EndTime = DateTime.UtcNow.AddHours(-1),
                RouteStatus = "Completed",
                EstimatedDistance = 25.5f,
                ActualDistance = 26.1f,
                RiskLevel = "Medium"
            };
            var r2 = new Routes
            {
                DriverId = driver.DriverId,
                Start = "Depot A",
                End = "Client C",
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddHours(1),
                RouteStatus = "InProgress",
                EstimatedDistance = 15.0f,
                ActualDistance = 0f,
                RiskLevel = "High"
            };
            _db.Routes.AddRange(r1, r2);
            await _db.SaveChangesAsync();
        }

        var routeActive = await _db.Routes.OrderByDescending(r => r.RouteId).FirstAsync();

        // Panic Alerts
        if (!await _db.PanicAlerts.AnyAsync())
        {
            _db.PanicAlerts.Add(new PanicAlerts
            {
                DriverId = driver.DriverId,
                RouteId = routeActive.RouteId,
                Alert = DateTime.UtcNow.AddMinutes(-30),
                Latitude = -34.0f,
                Longitude = 18.45f,
                Status = "Resolved",
                AlertType = "PanicButton",
                ResolvedTime = DateTime.UtcNow.AddMinutes(-10),
                Notes = "Driver pressed panic button, dispatcher called to confirm safety"
            });
        }
        await _db.SaveChangesAsync();

        var alertOpt = await _db.PanicAlerts.FirstOrDefaultAsync();

        // Incidents
        if (!await _db.Incidents.AnyAsync())
        {
            _db.Incidents.AddRange(
                new Incidents
                {
                    DriverId = driver.DriverId,
                    AlertId = alertOpt?.AlertId,
                    RouteId = routeActive.RouteId,
                    ZoneId = zone.ZoneId,
                    Type = "Delay",
                    Latitude = zone.Latitude,
                    Longitude = zone.Longitude,
                    Severity = "Low",
                    IncidentTime = DateTime.UtcNow.AddHours(-2),
                    Status = "Resolved"
                },
                new Incidents
                {
                    DriverId = driver.DriverId,
                    AlertId = null,
                    RouteId = routeActive.RouteId,
                    ZoneId = zone.ZoneId,
                    Type = "Theft Attempt",
                    Latitude = zone.Latitude + 0.01f,
                    Longitude = zone.Longitude + 0.01f,
                    Severity = "High",
                    IncidentTime = DateTime.UtcNow.AddMinutes(-45),
                    Status = "Investigating"
                }
            );
        }
        await _db.SaveChangesAsync();

        var incident = await _db.Incidents.OrderByDescending(i => i.IncidentId).FirstAsync();

        // Incident Responses
        if (!await _db.IncidentResponses.AnyAsync())
        {
            _db.IncidentResponses.Add(new IncidentResponses
            {
                IncidentId = incident.IncidentId,
                DriverId = driver.DriverId,
                Type = "PhoneCall",
                Time = DateTime.UtcNow.AddMinutes(-40),
                Details = "Dispatcher called driver to verify situation",
                WasSuccessful = true
            });
        }

        // Notifications
        if (!await _db.Notifications.AnyAsync())
        {
            _db.Notifications.AddRange(
                new Notifications { DriverId = driver.DriverId, Type = "Route", Title = "Route Started", Message = "Your route to Client C has begun", Priority = "Info", SentAt = DateTime.UtcNow.AddHours(-1), IsRead = true },
                new Notifications { DriverId = driver.DriverId, Type = "Alert", Title = "Panic Alert", Message = "Panic button pressed", Priority = "High", SentAt = DateTime.UtcNow.AddMinutes(-30), IsRead = false }
            );
        }

        // Threat Detections
        if (!await _db.ThreatDetections.AnyAsync())
        {
            var td = new ThreatDetections
            {
                DriverId = driver.DriverId,
                RouteId = routeActive.RouteId,
                ConfidenceScore = 0.87,
                ManualReview = "Initial AI detection requires review.",
                ConfirmedThreat = false,
                DetectionTime = DateTime.UtcNow.AddMinutes(-20),
                ThreatType = "SuspiciousObject",
                CameraData = new byte[] { 1, 2, 3 },
                Latitude = zone.Latitude + 0.02f,
                Longitude = zone.Longitude + 0.02f
            };
            _db.ThreatDetections.Add(td);
            await _db.SaveChangesAsync();
            _db.CameraRecordings.Add(new CameraRecordings
            {
                DriverId = driver.DriverId,
                RouteId = routeActive.RouteId,
                ThreatId = td.ThreatId,
                FilePath = "/recordings/threat1.mp4",
                FileSize = 12.5f,
                Camera = "FrontDash",
                Quality = "720p",
                Evidence = false
            });
        }

        // Safety Scores
        if (!await _db.SafetyScores.AnyAsync())
        {
            _db.SafetyScores.Add(new SafetyScore
            {
                DriverId = driver.DriverId,
                CalculatedDate = DateTime.UtcNow.Date,
                OverallScore = 82,
                IncidentScore = 70,
                RouteCompletionScore = 95.5,
                TotalIncidents = await _db.Incidents.CountAsync(),
                TotalRoutes = await _db.Routes.CountAsync(),
                CompletedRoutes = await _db.Routes.CountAsync(r => r.RouteStatus == "Completed"),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            });
        }

        // Monthly Reports
        if (!await _db.MonthlyReports.AnyAsync())
        {
            _db.MonthlyReports.Add(new MonthlyReports
            {
                CompanyId = company.CompanyId,
                ReportDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                TotalRoutes = await _db.Routes.CountAsync(),
                CompletedRoutes = await _db.Routes.CountAsync(r => r.RouteStatus == "Completed"),
                TotalIncidents = await _db.Incidents.CountAsync(),
                ResolvedIncidents = await _db.Incidents.CountAsync(i => i.Status == "Resolved"),
                TotalDistance = await _db.Routes.SumAsync(r => r.ActualDistance),
                AverageSafetyScore = (await _db.SafetyScores.AnyAsync()) ? (int)await _db.SafetyScores.AverageAsync(s => s.OverallScore) : 0,
                TotalDeliveries = 120,
                SuccessfulDeliveries = 115,
                PanicAlerts = await _db.PanicAlerts.CountAsync(),
                ThreatDetections = await _db.ThreatDetections.CountAsync(),
                AverageResponseTime = 8.5f,
                ReportData = "{ 'summary': 'Monthly performance metrics' }"
            });
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Database seeded with dummy data");
    }

    private async Task<Users> EnsureUser(int companyId, string email, string role, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null) return user;
        user = new Users
        {
            CompanyId = companyId,
            Email = email,
            Password = _hasher.Hash(password),
            Role = role,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}
