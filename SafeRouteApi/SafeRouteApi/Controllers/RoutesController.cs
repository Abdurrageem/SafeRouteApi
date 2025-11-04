using Microsoft.AspNetCore.Mvc;
using SafeRouteApi.Models;

namespace SafeRouteApi.Controllers
{
    /// <summary>
    /// API Controller for managing routes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly ILogger<RoutesController> _logger;

        public RoutesController(ILogger<RoutesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: api/routes
        /// Get all routes
        /// </summary>
        [HttpGet(Name = "GetAllRoutes")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Getting all routes");

            // Mock data - replace with database call
            var routes = new List<Routes>
            {
                new Routes
                {
                    RouteId = 1,
                    DriverId = 1,
                    Start = "Cape Town City Centre",
                    End = "Stellenbosch",
                    StartTime = DateTime.Now.AddHours(-2),
                    EndTime = DateTime.Now.AddHours(-1),
                    RouteStatus = "Completed",
                    EstimatedDistance = 52.5f,
                    ActualDistance = 50.0f,
                    RiskLevel = "Medium"
                },
                new Routes
                {
                    RouteId = 2,
                    DriverId = 1,
                    Start = "Khayelitsha",
                    End = "Mitchell's Plain",
                    StartTime = DateTime.Now.AddMinutes(-30),
                    RouteStatus = "InProgress",
                    EstimatedDistance = 12.3f,
                    ActualDistance = 0f,
                    RiskLevel = "High"
                },
                new Routes
                {
                    RouteId = 3,
                    DriverId = 2,
                    Start = "Bellville",
                    End = "Paarl",
                    StartTime = DateTime.Now.AddDays(1),
                    RouteStatus = "Planned",
                    EstimatedDistance = 45.0f,
                    ActualDistance = 0f,
                    RiskLevel = "Low"
                }
            };

            return Ok(routes);
        }

        /// <summary>
        /// GET: api/routes/{id}
        /// Get route by ID
        /// </summary>
        [HttpGet("{id}", Name = "GetRouteById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting route with ID: {id}");

            if (id <= 0)
            {
                return NotFound(new { message = $"Route with ID {id} not found" });
            }

            var route = new Routes
            {
                RouteId = id,
                DriverId = 1,
                Start = "Cape Town",
                End = "Stellenbosch",
                StartTime = DateTime.Now.AddHours(-2),
                EndTime = DateTime.Now.AddHours(-1),
                RouteStatus = "Completed",
                EstimatedDistance = 52.5f,
                ActualDistance = 50.1f,
                RiskLevel = "Medium"
            };

            return Ok(route);
        }

        /// <summary>
        /// GET: api/routes/driver/{driverId}
        /// Get all routes for a specific driver
        /// </summary>
        [HttpGet("driver/{driverId}", Name = "GetRoutesByDriver")]
        public IActionResult GetByDriverId(int driverId)
        {
            _logger.LogInformation($"Getting routes for driver: {driverId}");

            var routes = new List<Routes>
            {
                new Routes
                {
                    RouteId = 1,
                    DriverId = driverId,
                    Start = "Cape Town City Centre",
                    End = "Stellenbosch",
                    StartTime = DateTime.Now.AddHours(-2),
                    RouteStatus = "Completed",
                    EstimatedDistance = 52.5f,
                    ActualDistance = 50.0f,
                    RiskLevel = "Medium"
                }
            };

            return Ok(routes);
        }

        /// <summary>
        /// GET: api/routes/driver/{driverId}/active
        /// Get active routes for a specific driver
        /// </summary>
        [HttpGet("driver/{driverId}/active", Name = "GetActiveRoutes")]
        public IActionResult GetActiveRoutes(int driverId)
        {
            _logger.LogInformation($"Getting active routes for driver: {driverId}");

            var routes = new List<Routes>
            {
                new Routes
                {
                    RouteId = 2,
                    DriverId = driverId,
                    Start = "Khayelitsha",
                    End = "Mitchell's Plain",
                    StartTime = DateTime.Now.AddMinutes(-30),
                    RouteStatus = "InProgress",
                    EstimatedDistance = 12.3f,
                    ActualDistance = 3.1f,
                    RiskLevel = "High"
                }
            };

            return Ok(routes);
        }

        /// <summary>
        /// POST: api/routes
        /// Create a new route
        /// </summary>
        [HttpPost(Name = "CreateRoute")]
        public IActionResult Create([FromBody] CreateRouteDto dto)
        {
            _logger.LogInformation($"Creating route for driver {dto.DriverId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRoute = new Routes
            {
                RouteId = 999, // Would be generated by database
                DriverId = dto.DriverId,
                Start = dto.Start,
                End = dto.End,
                StartTime = dto.StartTime ?? DateTime.Now,
                RouteStatus = "Planned",
                EstimatedDistance = 0f,
                ActualDistance = 0f,
                RiskLevel = "Low"
            };

            return CreatedAtRoute(
                "GetRouteById",
                new { id = createdRoute.RouteId },
                createdRoute
            );
        }

        /// <summary>
        /// PUT: api/routes/{id}/status
        /// Update route status
        /// </summary>
        [HttpPut("{id}/status", Name = "UpdateRouteStatus")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateRouteStatusDto dto)
        {
            _logger.LogInformation($"Updating status for route {id} to {dto.RouteStatus}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update in database
            var updatedRoute = new Routes
            {
                RouteId = id,
                DriverId = 1,
                Start = "Cape Town",
                End = "Stellenbosch",
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = dto.EndTime ?? DateTime.Now,
                RouteStatus = dto.RouteStatus,
                EstimatedDistance = 52.5f,
                ActualDistance = 51.7f,
                RiskLevel = "Medium"
            };

            return Ok(updatedRoute);
        }

        /// <summary>
        /// PUT: api/routes/{id}/start
        /// Start a planned route
        /// </summary>
        [HttpPut("{id}/start", Name = "StartRoute")]
        public IActionResult StartRoute(int id)
        {
            _logger.LogInformation($"Starting route {id}");

            // TODO: Update in database
            return NoContent();
        }

        /// <summary>
        /// PUT: api/routes/{id}/complete
        /// Complete an active route
        /// </summary>
        [HttpPut("{id}/complete", Name = "CompleteRoute")]
        public IActionResult CompleteRoute(int id)
        {
            _logger.LogInformation($"Completing route {id}");

            // TODO: Update in database
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/routes/{id}
        /// Delete a route
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteRoute")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting route {id}");

            // TODO: Delete from database
            return NoContent();
        }

        /// <summary>
        /// GET: api/routes/driver/{driverId}/stats
        /// Get route statistics for a driver
        /// </summary>
        [HttpGet("driver/{driverId}/stats", Name = "GetRouteStats")]
        public IActionResult GetStats(int driverId)
        {
            _logger.LogInformation($"Getting route stats for driver {driverId}");

            var stats = new
            {
                DriverId = driverId,
                TotalRoutes = 45,
                CompletedRoutes = 42,
                ActiveRoutes = 1,
                PlannedRoutes = 2,
                TotalDistance = 2150.5,
                TotalDuration = 3600,
                AverageSafetyScore = 4.3,
                TotalRiskZonesEncountered = 38
            };

            return Ok(stats);
        }
    }

    public class CreateRouteDto
    {
        public int DriverId { get; set; }
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
    }

    public class UpdateRouteStatusDto
    {
        public string RouteStatus { get; set; } = string.Empty; // InProgress, Completed, Cancelled
        public DateTime? EndTime { get; set; }
    }
}
