using Microsoft.AspNetCore.Mvc;
using SafeRouteApi.Models;

namespace SafeRouteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly ILogger<DriversController> _logger;

        public DriversController(ILogger<DriversController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: api/drivers
        /// </summary>
        [HttpGet(Name = "GetAllDrivers")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Getting all drivers");

            var drivers = new List<Driver>
            {
                new Driver
                {
                    Id = 1,
                    UserId = 1,
                    Name = "John",
                    Surname = "Mbeki",
                    LicenseNumber = "CA-123-456",
                    VehicleRegistration = "CAW 12345",
                    VehicleModel = "Toyota Hilux 2021"
                },
                new Driver
                {
                    Id = 2,
                    UserId = 2,
                    Name = "Sarah",
                    Surname = "van der Walt",
                    LicenseNumber = "CA-789-012",
                    VehicleRegistration = "CAW 67890",
                    VehicleModel = "Ford Ranger 2022"
                }
            };

            return Ok(drivers);
        }

        /// <summary>
        /// GET: api/drivers/{id}
        /// </summary>
        [HttpGet("{id}", Name = "GetDriverById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting driver with ID: {id}");

            var driver = new Driver
            {
                Id = id,
                UserId = 1,
                Name = "John",
                Surname = "Mbeki",
                LicenseNumber = "CA-123-456",
                VehicleRegistration = "CAW 12345",
                VehicleModel = "Toyota Hilux 2021"
            };

            return Ok(driver);
        }

        /// <summary>
        /// GET: api/drivers/{id}/stats
        /// </summary>
        [HttpGet("{id}/stats", Name = "GetDriverStats")]
        public IActionResult GetStats(int id)
        {
            _logger.LogInformation($"Getting stats for driver {id}");

            var stats = new DriverStats
            {
                DriverId = id,
                Name = "John Mbeki",
                TotalRoutes = 45,
                CompletedRoutes = 42,
                ActiveRoutes = 1,
                TotalNotifications = 23,
                UnreadNotifications = 5,
                SafetyScore = 4.8
            };

            return Ok(stats);
        }
    }

    public class DriverStats
    {
        public int DriverId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalRoutes { get; set; }
        public int CompletedRoutes { get; set; }
        public int ActiveRoutes { get; set; }
        public int TotalNotifications { get; set; }
        public int UnreadNotifications { get; set; }
        public double SafetyScore { get; set; }
    }
}
