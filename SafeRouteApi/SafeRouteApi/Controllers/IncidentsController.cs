using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Controllers
{
    /// <summary>
    /// API Controller for incident reporting - Incident reporting
    /// Phase 3: Emergency Systems
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private readonly ILogger<IncidentsController> _logger;

        public IncidentsController(ILogger<IncidentsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST: api/incidents
        /// Report a new incident
        /// </summary>
        [HttpPost(Name = "ReportIncident")]
        public IActionResult ReportIncident([FromBody] ReportIncidentDto dto)
        {
            _logger.LogInformation($"New incident reported by driver {dto.DriverId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var incident = new Incident
            {
                Id = 1,
                DriverId = dto.DriverId,
                IncidentType = dto.IncidentType,
                Severity = dto.Severity,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Location = dto.Location,
                Description = dto.Description,
                OccurredAt = dto.OccurredAt ?? DateTime.Now,
                ReportedAt = DateTime.Now,
                Status = "Reported",
                IsVerified = false,
                IsActive = true
            };

            // TODO: Save to database and create risk zone if severity is high

            return CreatedAtRoute("GetIncidentById", new { id = incident.Id }, incident);
        }

        /// <summary>
        /// GET: api/incidents
        /// Get all incidents
        /// </summary>
        [HttpGet(Name = "GetAllIncidents")]
        public IActionResult GetAll([FromQuery] string? type = null, [FromQuery] string? severity = null, [FromQuery] bool? verified = null)
        {
            _logger.LogInformation($"Getting incidents (type: {type}, severity: {severity}, verified: {verified})");

            var incidents = new List<Incident>
            {
                new Incident
                {
                    Id = 1,
                    DriverId = 1,
                    DriverName = "John Mbeki",
                    IncidentType = "Crime",
                    Severity = "High",
                    Latitude = -34.0276,
                    Longitude = 18.5881,
                    Location = "Nyanga, Cape Town",
                    Description = "Armed robbery attempt near traffic light",
                    OccurredAt = DateTime.Now.AddHours(-2),
                    ReportedAt = DateTime.Now.AddHours(-1),
                    Status = "Verified",
                    IsVerified = true,
                    IsActive = true,
                    VerifiedBy = "Admin",
                    VerifiedAt = DateTime.Now.AddMinutes(-30)
                },
                new Incident
                {
                    Id = 2,
                    DriverId = 2,
                    DriverName = "Sarah van der Walt",
                    IncidentType = "Accident",
                    Severity = "Medium",
                    Latitude = -33.9249,
                    Longitude = 18.4241,
                    Location = "N1 Highway, Cape Town",
                    Description = "Multi-vehicle collision causing traffic jam",
                    OccurredAt = DateTime.Now.AddHours(-5),
                    ReportedAt = DateTime.Now.AddHours(-4),
                    Status = "Resolved",
                    IsVerified = true,
                    IsActive = false
                },
                new Incident
                {
                    Id = 3,
                    DriverId = 1,
                    DriverName = "John Mbeki",
                    IncidentType = "Suspicious Activity",
                    Severity = "Low",
                    Latitude = -34.0022,
                    Longitude = 18.5946,
                    Location = "Philippi, Cape Town",
                    Description = "Group loitering near intersection",
                    OccurredAt = DateTime.Now.AddDays(-1),
                    ReportedAt = DateTime.Now.AddDays(-1),
                    Status = "Under Review",
                    IsVerified = false,
                    IsActive = true
                }
            };

            // Apply filters
            if (!string.IsNullOrEmpty(type))
            {
                incidents = incidents.Where(i => i.IncidentType.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(severity))
            {
                incidents = incidents.Where(i => i.Severity.Equals(severity, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (verified.HasValue)
            {
                incidents = incidents.Where(i => i.IsVerified == verified.Value).ToList();
            }

            return Ok(incidents);
        }

        /// <summary>
        /// GET: api/incidents/{id}
        /// Get incident by ID
        /// </summary>
        [HttpGet("{id}", Name = "GetIncidentById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting incident {id}");

            var incident = new Incident
            {
                Id = id,
                DriverId = 1,
                DriverName = "John Mbeki",
                VehicleRegistration = "CAW 12345",
                IncidentType = "Crime",
                Severity = "High",
                Latitude = -34.0276,
                Longitude = 18.5881,
                Location = "Nyanga, Cape Town",
                Description = "Armed robbery attempt near traffic light",
                OccurredAt = DateTime.Now.AddHours(-2),
                ReportedAt = DateTime.Now.AddHours(-1),
                Status = "Verified",
                IsVerified = true,
                IsActive = true,
                VerifiedBy = "Admin",
                VerifiedAt = DateTime.Now.AddMinutes(-30),
                Images = new List<string> { "incident1_img1.jpg", "incident1_img2.jpg" },
                Witnesses = 2,
                PoliceNotified = true,
                PoliceReferenceNumber = "CAS-2024-12345"
            };

            return Ok(incident);
        }

        /// <summary>
        /// GET: api/incidents/driver/{driverId}
        /// Get incidents reported by a specific driver
        /// </summary>
        [HttpGet("driver/{driverId}", Name = "GetIncidentsByDriver")]
        public IActionResult GetByDriver(int driverId)
        {
            _logger.LogInformation($"Getting incidents for driver {driverId}");

            var incidents = new List<Incident>
            {
                new Incident
                {
                    Id = 1,
                    DriverId = driverId,
                    IncidentType = "Crime",
                    Severity = "High",
                    Location = "Nyanga, Cape Town",
                    OccurredAt = DateTime.Now.AddHours(-2),
                    Status = "Verified",
                    IsVerified = true
                }
            };

            return Ok(incidents);
        }

        /// <summary>
        /// GET: api/incidents/nearby
        /// Get incidents near a location
        /// </summary>
        [HttpGet("nearby", Name = "GetNearbyIncidents")]
        public IActionResult GetNearby([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radiusKm = 5.0, [FromQuery] int hoursAgo = 24)
        {
            _logger.LogInformation($"Getting incidents within {radiusKm}km of {latitude}, {longitude} in last {hoursAgo} hours");

            var incidents = new List<object>
            {
                new
                {
                    Incident = new Incident
                    {
                        Id = 1,
                        IncidentType = "Crime",
                        Severity = "High",
                        Latitude = -34.0276,
                        Longitude = 18.5881,
                        Location = "Nyanga, Cape Town",
                        OccurredAt = DateTime.Now.AddHours(-2),
                        IsVerified = true
                    },
                    DistanceKm = 2.3
                }
            };

            return Ok(incidents);
        }

        /// <summary>
        /// GET: api/incidents/recent
        /// Get recent incidents (last 24 hours)
        /// </summary>
        [HttpGet("recent", Name = "GetRecentIncidents")]
        public IActionResult GetRecent([FromQuery] int hours = 24)
        {
            _logger.LogInformation($"Getting incidents from last {hours} hours");

            var incidents = new List<Incident>
            {
                new Incident
                {
                    Id = 1,
                    DriverId = 1,
                    DriverName = "John Mbeki",
                    IncidentType = "Crime",
                    Severity = "High",
                    Location = "Nyanga, Cape Town",
                    OccurredAt = DateTime.Now.AddHours(-2),
                    Status = "Verified",
                    IsVerified = true
                }
            };

            return Ok(incidents);
        }

        /// <summary>
        /// GET: api/incidents/type/{type}
        /// Get incidents by type
        /// </summary>
        [HttpGet("type/{type}", Name = "GetIncidentsByType")]
        public IActionResult GetByType(string type)
        {
            _logger.LogInformation($"Getting incidents of type: {type}");

            var incidents = new List<Incident>
            {
                new Incident
                {
                    Id = 1,
                    IncidentType = type,
                    Severity = "High",
                    Location = "Cape Town",
                    OccurredAt = DateTime.Now.AddHours(-2),
                    IsVerified = true
                }
            };

            return Ok(incidents);
        }

        /// <summary>
        /// PUT: api/incidents/{id}
        /// Update incident information
        /// </summary>
        [HttpPut("{id}", Name = "UpdateIncident")]
        public IActionResult Update(int id, [FromBody] UpdateIncidentDto dto)
        {
            _logger.LogInformation($"Updating incident {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update incident in database
            return NoContent();
        }

        /// <summary>
        /// PUT: api/incidents/{id}/verify
        /// Verify an incident (admin/moderator)
        /// </summary>
        [HttpPut("{id}/verify", Name = "VerifyIncident")]
        public IActionResult Verify(int id, [FromBody] VerifyIncidentDto dto)
        {
            _logger.LogInformation($"Verifying incident {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Set IsVerified to true, add verifier info
            return Ok(new { message = "Incident verified" });
        }

        /// <summary>
        /// PUT: api/incidents/{id}/resolve
        /// Mark incident as resolved
        /// </summary>
        [HttpPut("{id}/resolve", Name = "ResolveIncident")]
        public IActionResult Resolve(int id, [FromBody] ResolveIncidentDto dto)
        {
            _logger.LogInformation($"Resolving incident {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update status to resolved
            return Ok(new { message = "Incident resolved" });
        }

        /// <summary>
        /// DELETE: api/incidents/{id}
        /// Delete an incident (admin only)
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteIncident")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting incident {id}");

            // TODO: Delete from database or mark as inactive
            return NoContent();
        }

        /// <summary>
        /// GET: api/incidents/stats
        /// Get incident statistics
        /// </summary>
        [HttpGet("stats", Name = "GetIncidentStats")]
        public IActionResult GetStats()
        {
            _logger.LogInformation("Getting incident statistics");

            var stats = new
            {
                TotalIncidents = 156,
                VerifiedIncidents = 128,
                UnverifiedIncidents = 28,
                ActiveIncidents = 45,
                ResolvedIncidents = 111,
                IncidentsByType = new
                {
                    Crime = 65,
                    Accident = 42,
                    SuspiciousActivity = 28,
                    RoadHazard = 12,
                    Other = 9
                },
                IncidentsBySeverity = new
                {
                    Critical = 15,
                    High = 48,
                    Medium = 67,
                    Low = 26
                },
                RecentTrends = new
                {
                    Today = 8,
                    ThisWeek = 34,
                    ThisMonth = 78,
                    LastMonth = 89
                }
            };

            return Ok(stats);
        }

        /// <summary>
        /// GET: api/incidents/heatmap
        /// Get incident data for heatmap visualization
        /// </summary>
        [HttpGet("heatmap", Name = "GetIncidentHeatmap")]
        public IActionResult GetHeatmap([FromQuery] int daysAgo = 30)
        {
            _logger.LogInformation($"Getting incident heatmap data for last {daysAgo} days");

            var heatmapData = new List<HeatmapPoint>
            {
                new HeatmapPoint { Latitude = -34.0276, Longitude = 18.5881, Weight = 15 },
                new HeatmapPoint { Latitude = -34.0290, Longitude = 18.6790, Weight = 12 },
                new HeatmapPoint { Latitude = -34.0022, Longitude = 18.5946, Weight = 8 }
            };

            return Ok(heatmapData);
        }
    }

    // Incident Models
    public class Incident
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string VehicleRegistration { get; set; } = string.Empty;
        public string IncidentType { get; set; } = string.Empty; // Crime, Accident, Suspicious Activity, Road Hazard, Weather, Other
        public string Severity { get; set; } = string.Empty; // Critical, High, Medium, Low
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public DateTime ReportedAt { get; set; }
        public string Status { get; set; } = string.Empty; // Reported, Under Review, Verified, Resolved
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public List<string>? Images { get; set; }
        public int? Witnesses { get; set; }
        public bool? PoliceNotified { get; set; }
        public string? PoliceReferenceNumber { get; set; }
    }

    public class HeatmapPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Weight { get; set; }
    }

    // DTOs
    public class ReportIncidentDto
    {
        [Required]
        public int DriverId { get; set; }

        [Required]
        public string IncidentType { get; set; } = string.Empty;

        [Required]
        public string Severity { get; set; } = string.Empty;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string Location { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime? OccurredAt { get; set; }

        public bool PoliceNotified { get; set; } = false;

        public string? PoliceReferenceNumber { get; set; }
    }

    public class UpdateIncidentDto
    {
        public string? Description { get; set; }
        public string? Status { get; set; }
        public bool? PoliceNotified { get; set; }
        public string? PoliceReferenceNumber { get; set; }
    }

    public class VerifyIncidentDto
    {
        [Required]
        public string VerifiedBy { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }

    public class ResolveIncidentDto
    {
        [Required]
        public string ResolutionNotes { get; set; } = string.Empty;
    }
}
