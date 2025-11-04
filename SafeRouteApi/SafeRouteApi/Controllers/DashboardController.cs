using Microsoft.AspNetCore.Mvc;

namespace SafeRouteApi.Controllers
{
    /// <summary>
    /// API Controller for dashboard - Main app entry point
    /// Phase 1: Core Functionality
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: api/dashboard
        /// Get dashboard overview for current user
        /// </summary>
        [HttpGet(Name = "GetDashboard")]
        public IActionResult GetDashboard()
        {
            _logger.LogInformation("Getting dashboard data");

            var dashboard = new DashboardData
            {
                DriverInfo = new DriverSummary
                {
                    DriverId = 1,
                    Name = "John Mbeki",
                    VehicleRegistration = "CAW 12345",
                    VehicleModel = "Toyota Hilux 2021",
                    SafetyScore = 4.8,
                    Status = "Active"
                },
                ActiveRoute = new ActiveRouteSummary
                {
                    RouteId = 1,
                    StartLocation = "Cape Town CBD",
                    EndLocation = "Stellenbosch",
                    StartTime = DateTime.Now.AddHours(-1),
                    EstimatedArrival = DateTime.Now.AddHours(1),
                    DistanceKm = 45.2,
                    Status = "In Progress",
                    CurrentProgress = 45
                },
                Stats = new DashboardStats
                {
                    TotalRoutes = 45,
                    CompletedToday = 3,
                    ActiveRoutes = 1,
                    UnreadNotifications = 5,
                    NearbyRiskZones = 2,
                    SafetyAlerts = 1
                },
                RecentNotifications = new List<NotificationSummary>
                {
                    new NotificationSummary
                    {
                        Id = 1,
                        Type = "Warning",
                        Title = "Risk Zone Ahead",
                        Message = "Approaching high-risk area in 2km",
                        Timestamp = DateTime.Now.AddMinutes(-15),
                        IsRead = false
                    },
                    new NotificationSummary
                    {
                        Id = 2,
                        Type = "Info",
                        Title = "Route Update",
                        Message = "Alternative route suggested",
                        Timestamp = DateTime.Now.AddMinutes(-30),
                        IsRead = false
                    }
                },
                NearbyRiskZones = new List<RiskZoneSummary>
                {
                    new RiskZoneSummary
                    {
                        Id = 1,
                        Name = "Nyanga",
                        RiskLevel = "High",
                        RiskType = "Crime",
                        DistanceKm = 2.3
                    },
                    new RiskZoneSummary
                    {
                        Id = 2,
                        Name = "Khayelitsha",
                        RiskLevel = "High",
                        RiskType = "Crime",
                        DistanceKm = 4.1
                    }
                }
            };

            return Ok(dashboard);
        }

        /// <summary>
        /// GET: api/dashboard/stats
        /// Get detailed statistics for dashboard
        /// </summary>
        [HttpGet("stats", Name = "GetDashboardStats")]
        public IActionResult GetStats()
        {
            _logger.LogInformation("Getting dashboard statistics");

            var stats = new DetailedStats
            {
                Today = new DailyStats
                {
                    RoutesCompleted = 3,
                    TotalDistanceKm = 135.6,
                    AverageSpeed = 65.2,
                    SafetyScore = 4.9,
                    RiskZonesEncountered = 5
                },
                ThisWeek = new WeeklyStats
                {
                    RoutesCompleted = 18,
                    TotalDistanceKm = 842.3,
                    AverageSpeed = 67.8,
                    SafetyScore = 4.7,
                    RiskZonesEncountered = 23
                },
                ThisMonth = new MonthlyStats
                {
                    RoutesCompleted = 72,
                    TotalDistanceKm = 3421.5,
                    AverageSpeed = 68.4,
                    SafetyScore = 4.8,
                    RiskZonesEncountered = 89
                }
            };

            return Ok(stats);
        }

        /// <summary>
        /// GET: api/dashboard/activity
        /// Get recent activity feed
        /// </summary>
        [HttpGet("activity", Name = "GetRecentActivity")]
        public IActionResult GetRecentActivity([FromQuery] int limit = 10)
        {
            _logger.LogInformation($"Getting recent activity (limit: {limit})");

            var activities = new List<ActivityItem>
            {
                new ActivityItem
                {
                    Id = 1,
                    Type = "Route",
                    Action = "Completed",
                    Description = "Route from Cape Town to Stellenbosch completed",
                    Timestamp = DateTime.Now.AddHours(-2),
                    Icon = "check-circle",
                    Color = "green"
                },
                new ActivityItem
                {
                    Id = 2,
                    Type = "Alert",
                    Action = "Warning",
                    Description = "Risk zone detected near your route",
                    Timestamp = DateTime.Now.AddHours(-3),
                    Icon = "alert-triangle",
                    Color = "orange"
                },
                new ActivityItem
                {
                    Id = 3,
                    Type = "Route",
                    Action = "Started",
                    Description = "Route from Paarl to Cape Town started",
                    Timestamp = DateTime.Now.AddHours(-5),
                    Icon = "navigation",
                    Color = "blue"
                }
            };

            return Ok(activities.Take(limit));
        }

        /// <summary>
        /// GET: api/dashboard/alerts
        /// Get active safety alerts
        /// </summary>
        [HttpGet("alerts", Name = "GetActiveSafetyAlerts")]
        public IActionResult GetAlerts()
        {
            _logger.LogInformation("Getting active safety alerts");

            var alerts = new List<SafetyAlert>
            {
                new SafetyAlert
                {
                    Id = 1,
                    Severity = "High",
                    Type = "Risk Zone",
                    Title = "High-Risk Area Nearby",
                    Message = "You are within 2km of a high-risk crime area",
                    Location = "Nyanga",
                    Timestamp = DateTime.Now.AddMinutes(-15),
                    IsActive = true,
                    ActionRequired = "Consider alternative route"
                }
            };

            return Ok(alerts);
        }

        /// <summary>
        /// GET: api/dashboard/quick-stats
        /// Get quick statistics for dashboard widgets
        /// </summary>
        [HttpGet("quick-stats", Name = "GetQuickStats")]
        public IActionResult GetQuickStats()
        {
            _logger.LogInformation("Getting quick stats");

            var quickStats = new
            {
                RoutesToday = 3,
                DistanceToday = 135.6,
                SafetyScore = 4.8,
                ActiveAlerts = 1,
                UnreadNotifications = 5,
                LastRouteCompleted = DateTime.Now.AddHours(-2)
            };

            return Ok(quickStats);
        }
    }

    // Dashboard Data Models
    public class DashboardData
    {
        public DriverSummary DriverInfo { get; set; } = new();
        public ActiveRouteSummary? ActiveRoute { get; set; }
        public DashboardStats Stats { get; set; } = new();
        public List<NotificationSummary> RecentNotifications { get; set; } = new();
        public List<RiskZoneSummary> NearbyRiskZones { get; set; } = new();
    }

    public class DriverSummary
    {
        public int DriverId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VehicleRegistration { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public double SafetyScore { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ActiveRouteSummary
    {
        public int RouteId { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public double DistanceKm { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CurrentProgress { get; set; }
    }

    public class DashboardStats
    {
        public int TotalRoutes { get; set; }
        public int CompletedToday { get; set; }
        public int ActiveRoutes { get; set; }
        public int UnreadNotifications { get; set; }
        public int NearbyRiskZones { get; set; }
        public int SafetyAlerts { get; set; }
    }

    public class NotificationSummary
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }

    public class RiskZoneSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public string RiskType { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
    }

    public class DetailedStats
    {
        public DailyStats Today { get; set; } = new();
        public WeeklyStats ThisWeek { get; set; } = new();
        public MonthlyStats ThisMonth { get; set; } = new();
    }

    public class DailyStats
    {
        public int RoutesCompleted { get; set; }
        public double TotalDistanceKm { get; set; }
        public double AverageSpeed { get; set; }
        public double SafetyScore { get; set; }
        public int RiskZonesEncountered { get; set; }
    }

    public class WeeklyStats
    {
        public int RoutesCompleted { get; set; }
        public double TotalDistanceKm { get; set; }
        public double AverageSpeed { get; set; }
        public double SafetyScore { get; set; }
        public int RiskZonesEncountered { get; set; }
    }

    public class MonthlyStats
    {
        public int RoutesCompleted { get; set; }
        public double TotalDistanceKm { get; set; }
        public double AverageSpeed { get; set; }
        public double SafetyScore { get; set; }
        public int RiskZonesEncountered { get; set; }
    }

    public class ActivityItem
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    public class SafetyAlert
    {
        public int Id { get; set; }
        public string Severity { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsActive { get; set; }
        public string ActionRequired { get; set; } = string.Empty;
    }
}
