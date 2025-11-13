using Microsoft.EntityFrameworkCore;
using SafeRoute.Data;
using SafeRoute.Models;

namespace SafeRoute.Services;

public class SeedData
{
    private readonly SafeRouteDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly ILogger<SeedData> _logger;

    public SeedData(SafeRouteDbContext db, IPasswordHasher hasher, ILogger<SeedData> logger)
    { _db = db; _hasher = hasher; _logger = logger; }

    public async Task InitializeAsync()
    {
        if (await _db.Users.AnyAsync()) return;

        var company = new Companies
        {
            Name = "SafeRoute Co",
            Address = "1 Safety Road, Cape Town",
            CompanyRegistration = "SR-0001",
            Email = "info@saferoute.local",
            ContactNumber = "+27000000000"
        };
        _db.Companies.Add(company);
        await _db.SaveChangesAsync();

        var adminUser = CreateUser(company.CompanyId, "admin@saferoute.local", "Admin", "Admin@12345");
        var dispatcherUser = CreateUser(company.CompanyId, "dispatcher@saferoute.local", "Dispatcher", "Dispatch@12345");
        var driverUser = CreateUser(company.CompanyId, "driver@saferoute.local", "Driver", "Driver@12345");
        _db.Users.AddRange(adminUser, dispatcherUser, driverUser);
        await _db.SaveChangesAsync();

        var dispatcher = new Dispatchers
        {
            UserId = dispatcherUser.UserId,
            ShiftStartTime = DateTime.UtcNow.Date.AddHours(6),
            ShiftPattern = "Day",
            IsOnDuty = true
        };
        _db.Dispatchers.Add(dispatcher);
        await _db.SaveChangesAsync();

        var driver = new Drivers
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
        _db.Drivers.Add(driver);
        await _db.SaveChangesAsync();

        _db.EmergencyContacts.AddRange(
            new EmergencyContacts { DriverId = driver.DriverId, Name = "Mary", Surname = "Mbeki", Relationship = "Spouse", PhoneNumber = "+27821234567", Email = "mary.mbeki@example.com" },
            new EmergencyContacts { DriverId = driver.DriverId, Name = "David", Surname = "Mbeki", Relationship = "Brother", PhoneNumber = "+27829876543", Email = "david.mbeki@example.com" }
        );

        _db.RiskZones.AddRange(
            new RiskZones { Name = "Nyanga", Latitude = -34.0276f, Longitude = 18.5881f, RiskLevel = "High", Incident = false, IncidentCount = 0 },
            new RiskZones { Name = "Khayelitsha", Latitude = -34.0290f, Longitude = 18.6790f, RiskLevel = "High", Incident = false, IncidentCount = 0 }
        );
        await _db.SaveChangesAsync();

        var zone = await _db.RiskZones.FirstAsync();

        var route = new Routes
        {
            DriverId = driver.DriverId,
            Start = "Depot A",
            End = "Client B",
            StartTime = DateTime.UtcNow.AddHours(-2),
            EndTime = DateTime.UtcNow.AddHours(-1),
            RouteStatus = "Completed",
            EstimatedDistance = 25.5f,
            ActualDistance = 26.1f,
            RiskLevel = "Medium"
        };
        _db.Routes.Add(route);
        await _db.SaveChangesAsync();

        var alert = new PanicAlerts
        {
            DriverId = driver.DriverId,
            RouteId = route.RouteId,
            Alert = DateTime.UtcNow.AddMinutes(-30),
            Latitude = -34.0f,
            Longitude = 18.45f,
            Status = "Resolved",
            AlertType = "PanicButton",
            ResolvedTime = DateTime.UtcNow.AddMinutes(-10),
            Notes = "Driver pressed panic button, dispatcher called to confirm safety"
        };
        _db.PanicAlerts.Add(alert);
        await _db.SaveChangesAsync();

        _db.Incidents.Add(new Incidents
        {
            DriverId = driver.DriverId,
            AlertId = alert.AlertId,
            RouteId = route.RouteId,
            ZoneId = zone.ZoneId,
            Type = "Delay",
            Latitude = zone.Latitude,
            Longitude = zone.Longitude,
            Severity = "Low",
            IncidentTime = DateTime.UtcNow.AddHours(-1),
            Status = "Resolved"
        });
        await _db.SaveChangesAsync();

        var incident = await _db.Incidents.FirstAsync();

        _db.IncidentResponses.Add(new IncidentResponses
        {
            IncidentId = incident.IncidentId,
            DriverId = driver.DriverId,
            Type = "PhoneCall",
            Time = DateTime.UtcNow.AddMinutes(-50),
            Details = "Dispatcher verified driver status",
            WasSuccessful = true
        });

        _db.Notifications.Add(new Notifications
        {
            DriverId = driver.DriverId,
            Type = "Route",
            Title = "Route Completed",
            Message = "Your route to Client B is completed",
            Priority = "Info",
            SentAt = DateTime.UtcNow,
            IsRead = true
        });

        var threat = new ThreatDetections
        {
            DriverId = driver.DriverId,
            RouteId = route.RouteId,
            ConfidenceScore = 0.87,
            ManualReview = "Initial detection.",
            ConfirmedThreat = false,
            DetectionTime = DateTime.UtcNow.AddMinutes(-20),
            ThreatType = "SuspiciousObject",
            CameraData = new byte[] { 1, 2 },
            Latitude = zone.Latitude + 0.01f,
            Longitude = zone.Longitude + 0.01f
        };
        _db.ThreatDetections.Add(threat);
        await _db.SaveChangesAsync();

        _db.CameraRecordings.Add(new CameraRecordings
        {
            DriverId = driver.DriverId,
            RouteId = route.RouteId,
            ThreatId = threat.ThreatId,
            FilePath = "/recordings/threat1.mp4",
            FileSize = 12.5f,
            Camera = "FrontDash",
            Quality = "720p",
            Evidence = false
        });

        _db.SafetyScores.Add(new SafetyScore
        {
            DriverId = driver.DriverId,
            CalculatedDate = DateTime.UtcNow.Date,
            OverallScore = 82,
            IncidentScore = 70,
            RouteCompletionScore = 95.5,
            TotalIncidents = 1,
            TotalRoutes = 1,
            CompletedRoutes = 1,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        });

        _db.MonthlyReports.Add(new MonthlyReports
        {
            CompanyId = company.CompanyId,
            ReportDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
            TotalRoutes = 1,
            CompletedRoutes = 1,
            TotalIncidents = 1,
            ResolvedIncidents = 1,
            TotalDistance = route.ActualDistance,
            AverageSafetyScore = 82,
            TotalDeliveries = 10,
            SuccessfulDeliveries = 9,
            PanicAlerts = 1,
            ThreatDetections = 1,
            AverageResponseTime = 8.5f,
            ReportData = "{ 'summary': 'Initial seed report' }"
        });

        await _db.SaveChangesAsync();
        _logger.LogInformation("Fresh SQLite database seeded");
    }

    private Users CreateUser(int companyId, string email, string role, string password) => new Users
    {
        CompanyId = companyId,
        Email = email,
        Password = _hasher.Hash(password),
        Role = role,
        CreatedAt = DateTime.UtcNow,
        UpdateAt = DateTime.UtcNow
    };
}
