using Microsoft.AspNetCore.Mvc;
using SafeRouteSharedLib.Models;

namespace SafeRouteApi.Controllers
{
    /// <summary>
    /// API Controller for managing risk zones
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RiskZonesController : ControllerBase
    {
        private readonly ILogger<RiskZonesController> _logger;

        public RiskZonesController(ILogger<RiskZonesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: api/riskzones
        /// Get all risk zones
        /// </summary>
        [HttpGet(Name = "GetAllRiskZones")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Getting all risk zones");

            var riskZones = new List<RiskZones>
            {
                new RiskZones { ZoneId = 1, Name = "Nyanga", Latitude = -34.0276f, Longitude = 18.5881f, IncidentCount = 45, LastIncident = DateTime.Now.AddDays(-2), RiskLevel = "High" },
                new RiskZones { ZoneId = 2, Name = "Khayelitsha", Latitude = -34.0290f, Longitude = 18.6790f, IncidentCount = 38, LastIncident = DateTime.Now.AddDays(-1), RiskLevel = "High" },
                new RiskZones { ZoneId = 3, Name = "Philippi", Latitude = -34.0022f, Longitude = 18.5946f, IncidentCount = 22, LastIncident = DateTime.Now.AddDays(-5), RiskLevel = "Medium" },
                new RiskZones { ZoneId = 4, Name = "Chapman's Peak Drive", Latitude = -34.0928f, Longitude = 18.3551f, IncidentCount = 12, LastIncident = DateTime.Now.AddDays(-10), RiskLevel = "Medium" },
                new RiskZones { ZoneId = 5, Name = "N1 - N2 Interchange", Latitude = -33.9416f, Longitude = 18.5041f, IncidentCount = 8, LastIncident = DateTime.Now.AddDays(-3), RiskLevel = "Low" }
            };

            return Ok(riskZones);
        }

        /// <summary>
        /// GET: api/riskzones/{id}
        /// Get risk zone by ID
        /// </summary>
        [HttpGet("{id}", Name = "GetRiskZoneById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting risk zone with ID: {id}");

            if (id <= 0)
            {
                return NotFound(new { message = $"Risk zone with ID {id} not found" });
            }

            var riskZone = new RiskZones
            {
                ZoneId = id,
                Name = "Nyanga",
                Latitude = -34.0276f,
                Longitude = 18.5881f,
                IncidentCount = 45,
                LastIncident = DateTime.Now.AddDays(-2),
                RiskLevel = "High"
            };

            return Ok(riskZone);
        }

        /// <summary>
        /// GET: api/riskzones/active
        /// Get all active risk zones (those with recent incidents)
        /// </summary>
        [HttpGet("active", Name = "GetActiveRiskZones")]
        public IActionResult GetActive()
        {
            _logger.LogInformation("Getting active risk zones");

            var riskZones = new List<RiskZones>
            {
                new RiskZones
                {
                    ZoneId = 1,
                    Name = "Nyanga",
                    Latitude = -34.0276f,
                    Longitude = 18.5881f,
                    IncidentCount = 45,
                    RiskLevel = "High"
                }
            };

            return Ok(riskZones);
        }

        /// <summary>
        /// GET: api/riskzones/level/{level}
        /// Get risk zones by risk level (Low, Medium, High)
        /// </summary>
        [HttpGet("level/{level}", Name = "GetRiskZonesByLevel")]
        public IActionResult GetByLevel(string level)
        {
            _logger.LogInformation($"Getting risk zones with level: {level}");

            var riskZones = new List<RiskZones>
            {
                new RiskZones
                {
                    ZoneId = 1,
                    Name = "Nyanga",
                    Latitude = -34.0276f,
                    Longitude = 18.5881f,
                    RiskLevel = level
                }
            };

            return Ok(riskZones);
        }

        /// <summary>
        /// GET: api/riskzones/type/{type}
        /// Get risk zones by type (placeholder; type not modeled in entity)
        /// </summary>
        [HttpGet("type/{type}", Name = "GetRiskZonesByType")]
        public IActionResult GetByType(string type)
        {
            _logger.LogInformation($"Getting risk zones with type: {type}");

            var riskZones = new List<RiskZones>
            {
                new RiskZones
                {
                    ZoneId = 1,
                    Name = "Sample Zone",
                    Latitude = -33.9249f,
                    Longitude = 18.4241f,
                    RiskLevel = "Medium"
                }
            };

            return Ok(riskZones);
        }

        /// <summary>
        /// POST: api/riskzones/check
        /// Check if a location is within any risk zones (mock)
        /// </summary>
        [HttpPost("check", Name = "CheckLocation")]
        public IActionResult CheckLocation([FromBody] CheckLocationDto dto)
        {
            _logger.LogInformation($"Checking location: {dto.Latitude}, {dto.Longitude}");

            var nearbyRiskZones = new List<object>
            {
                new
                {
                    RiskZone = new RiskZones
                    {
                        ZoneId = 1,
                        Name = "Nyanga",
                        Latitude = -34.0276f,
                        Longitude = 18.5881f,
                        RiskLevel = "High"
                    },
                    DistanceKm = 1.5,
                    IsWithinZone = true
                }
            };

            return Ok(new
            {
                Location = new { dto.Latitude, dto.Longitude },
                IsInRiskZone = true,
                NearbyRiskZones = nearbyRiskZones
            });
        }

        /// <summary>
        /// POST: api/riskzones
        /// Create a new risk zone
        /// </summary>
        [HttpPost(Name = "CreateRiskZone")]
        public IActionResult Create([FromBody] CreateRiskZoneDto dto)
        {
            _logger.LogInformation($"Creating risk zone: {dto.Name}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRiskZone = new RiskZones
            {
                ZoneId = 999, // Would be generated by database
                Name = dto.Name,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IncidentCount = 0,
                RiskLevel = dto.RiskLevel
            };

            return CreatedAtRoute(
                "GetRiskZoneById",
                new { id = createdRiskZone.ZoneId },
                createdRiskZone
            );
        }

        /// <summary>
        /// PUT: api/riskzones/{id}
        /// Update risk zone information
        /// </summary>
        [HttpPut("{id}", Name = "UpdateRiskZone")]
        public IActionResult Update(int id, [FromBody] UpdateRiskZoneDto dto)
        {
            _logger.LogInformation($"Updating risk zone {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update in database
            return NoContent();
        }

        /// <summary>
        /// PUT: api/riskzones/{id}/incident
        /// Report an incident in a risk zone (increments incident count)
        /// </summary>
        [HttpPut("{id}/incident", Name = "ReportRiskZoneIncident")]
        public IActionResult ReportIncident(int id)
        {
            _logger.LogInformation($"Reporting incident in risk zone {id}");

            // TODO: Update incident count and last incident date in database
            return NoContent();
        }

        /// <summary>
        /// PUT: api/riskzones/{id}/deactivate
        /// Deactivate a risk zone
        /// </summary>
        [HttpPut("{id}/deactivate", Name = "DeactivateRiskZone")]
        public IActionResult Deactivate(int id)
        {
            _logger.LogInformation($"Deactivating risk zone {id}");

            // TODO: Set IsActive to false in database (not modeled)
            return NoContent();
        }

        /// <summary>
        /// PUT: api/riskzones/{id}/activate
        /// Activate a risk zone
        /// </summary>
        [HttpPut("{id}/activate", Name = "ActivateRiskZone")]
        public IActionResult Activate(int id)
        {
            _logger.LogInformation($"Activating risk zone {id}");

            // TODO: Set IsActive to true in database (not modeled)
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/riskzones/{id}
        /// Delete a risk zone
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteRiskZone")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting risk zone {id}");

            // TODO: Delete from database
            return NoContent();
        }

        /// <summary>
        /// GET: api/riskzones/nearby
        /// Get risk zones within a certain radius of a location
        /// </summary>
        [HttpGet("nearby", Name = "GetNearbyRiskZones")]
        public IActionResult GetNearby([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radiusKm = 5.0)
        {
            _logger.LogInformation($"Getting risk zones within {radiusKm}km of {latitude}, {longitude}");

            var nearbyRiskZones = new List<object>
            {
                new
                {
                    RiskZone = new RiskZones
                    {
                        ZoneId = 1,
                        Name = "Nyanga",
                        Latitude = -34.0276f,
                        Longitude = 18.5881f,
                        RiskLevel = "High"
                    },
                    DistanceKm = 2.3
                }
            };

            return Ok(nearbyRiskZones);
        }

        /// <summary>
        /// GET: api/riskzones/stats
        /// Get overall risk zone statistics
        /// </summary>
        [HttpGet("stats", Name = "GetRiskZoneStats")]
        public IActionResult GetStats()
        {
            _logger.LogInformation("Getting risk zone statistics");

            var stats = new
            {
                TotalRiskZones = 45,
                ActiveRiskZones = 38,
                InactiveRiskZones = 7,
                HighRiskZones = 12,
                MediumRiskZones = 18,
                LowRiskZones = 8,
                TotalIncidents = 456
            };

            return Ok(stats);
        }
    }

    public class CheckLocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CreateRiskZoneDto
    {
        public string Name { get; set; } = string.Empty;
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string RiskLevel { get; set; } = "Medium";
    }

    public class UpdateRiskZoneDto
    {
        public string? RiskLevel { get; set; }
        public string? Description { get; set; }
        public int? IncidentCount { get; set; }
        public bool? IsActive { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
