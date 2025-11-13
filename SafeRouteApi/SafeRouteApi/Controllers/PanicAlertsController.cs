using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Controllers
{
    /// <summary>
    /// API Controller for panic alerts - SOS button
    /// Phase 3: Emergency Systems
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PanicAlertsController : ControllerBase
    {
        private readonly ILogger<PanicAlertsController> _logger;

        public PanicAlertsController(ILogger<PanicAlertsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST: api/panicalerts
        /// Trigger a panic alert / SOS
        /// </summary>
        [HttpPost(Name = "TriggerPanicAlert")]
        public IActionResult TriggerPanicAlert([FromBody] TriggerPanicAlertDto dto)
        {
            _logger.LogWarning($"PANIC ALERT TRIGGERED - Driver {dto.DriverId} at {dto.Latitude}, {dto.Longitude}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create panic alert
            var panicAlert = new PanicAlert
            {
                Id = 1,
                DriverId = dto.DriverId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Location = dto.Location,
                AlertType = dto.AlertType,
                Description = dto.Description,
                Status = "Active",
                Priority = "Critical",
                TriggeredAt = DateTime.Now,
                IsActive = true,
                ResponseTime = null,
                ResolvedAt = null
            };

            // TODO: Implement emergency response:
            // 1. Send SMS to emergency contacts
            // 2. Notify nearby security/police
            // 3. Alert dispatch/admin
            // 4. Start location tracking
            // 5. Send push notifications to emergency contacts

            return CreatedAtRoute("GetPanicAlertById", new { id = panicAlert.Id }, panicAlert);
        }

        /// <summary>
        /// GET: api/panicalerts
        /// Get all panic alerts (admin/dispatch)
        /// </summary>
        [HttpGet(Name = "GetAllPanicAlerts")]
        public IActionResult GetAll([FromQuery] string? status = null)
        {
            _logger.LogInformation($"Getting panic alerts (status: {status})");

            var alerts = new List<PanicAlert>
            {
                new PanicAlert
                {
                    Id = 1,
                    DriverId = 1,
                    DriverName = "John Mbeki",
                    VehicleRegistration = "CAW 12345",
                    Latitude = -34.0276,
                    Longitude = 18.5881,
                    Location = "Nyanga, Cape Town",
                    AlertType = "Emergency",
                    Description = "Vehicle breakdown in high-risk area",
                    Status = "Active",
                    Priority = "Critical",
                    TriggeredAt = DateTime.Now.AddMinutes(-15),
                    IsActive = true
                },
                new PanicAlert
                {
                    Id = 2,
                    DriverId = 2,
                    DriverName = "Sarah van der Walt",
                    VehicleRegistration = "CAW 67890",
                    Latitude = -34.0290,
                    Longitude = 18.6790,
                    Location = "Khayelitsha, Cape Town",
                    AlertType = "Threat",
                    Description = "Suspicious activity near vehicle",
                    Status = "Resolved",
                    Priority = "High",
                    TriggeredAt = DateTime.Now.AddHours(-2),
                    ResolvedAt = DateTime.Now.AddHours(-1),
                    IsActive = false
                }
            };

            if (!string.IsNullOrEmpty(status))
            {
                alerts = alerts.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(alerts);
        }

        /// <summary>
        /// GET: api/panicalerts/{id}
        /// Get panic alert by ID
        /// </summary>
        [HttpGet("{id}", Name = "GetPanicAlertById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting panic alert {id}");

            var alert = new PanicAlert
            {
                Id = id,
                DriverId = 1,
                DriverName = "John Mbeki",
                VehicleRegistration = "CAW 12345",
                Latitude = -34.0276,
                Longitude = 18.5881,
                Location = "Nyanga, Cape Town",
                AlertType = "Emergency",
                Description = "Vehicle breakdown in high-risk area",
                Status = "Active",
                Priority = "Critical",
                TriggeredAt = DateTime.Now.AddMinutes(-15),
                IsActive = true,
                EmergencyContactsNotified = new List<string> { "0821234567", "0829876543" },
                ResponseLog = new List<ResponseLogEntry>
                {
                    new ResponseLogEntry
                    {
                        Timestamp = DateTime.Now.AddMinutes(-14),
                        Action = "Alert triggered",
                        PerformedBy = "System"
                    },
                    new ResponseLogEntry
                    {
                        Timestamp = DateTime.Now.AddMinutes(-13),
                        Action = "Emergency contacts notified",
                        PerformedBy = "System"
                    }
                }
            };

            return Ok(alert);
        }

        /// <summary>
        /// GET: api/panicalerts/driver/{driverId}
        /// Get panic alerts for a specific driver
        /// </summary>
        [HttpGet("driver/{driverId}", Name = "GetPanicAlertsByDriver")]
        public IActionResult GetByDriver(int driverId)
        {
            _logger.LogInformation($"Getting panic alerts for driver {driverId}");

            var alerts = new List<PanicAlert>
            {
                new PanicAlert
                {
                    Id = 1,
                    DriverId = driverId,
                    DriverName = "John Mbeki",
                    Latitude = -34.0276,
                    Longitude = 18.5881,
                    Location = "Nyanga, Cape Town",
                    AlertType = "Emergency",
                    Status = "Active",
                    TriggeredAt = DateTime.Now.AddMinutes(-15),
                    IsActive = true
                }
            };

            return Ok(alerts);
        }

        /// <summary>
        /// GET: api/panicalerts/active
        /// Get all active panic alerts
        /// </summary>
        [HttpGet("active", Name = "GetActivePanicAlerts")]
        public IActionResult GetActive()
        {
            _logger.LogInformation("Getting active panic alerts");

            var alerts = new List<PanicAlert>
            {
                new PanicAlert
                {
                    Id = 1,
                    DriverId = 1,
                    DriverName = "John Mbeki",
                    VehicleRegistration = "CAW 12345",
                    Latitude = -34.0276,
                    Longitude = 18.5881,
                    Location = "Nyanga, Cape Town",
                    AlertType = "Emergency",
                    Description = "Vehicle breakdown in high-risk area",
                    Status = "Active",
                    Priority = "Critical",
                    TriggeredAt = DateTime.Now.AddMinutes(-15),
                    IsActive = true
                }
            };

            return Ok(alerts);
        }

        /// <summary>
        /// PUT: api/panicalerts/{id}/acknowledge
        /// Acknowledge a panic alert (dispatcher/admin)
        /// </summary>
        [HttpPut("{id}/acknowledge", Name = "AcknowledgePanicAlert")]
        public IActionResult Acknowledge(int id, [FromBody] AcknowledgeAlertDto dto)
        {
            _logger.LogInformation($"Acknowledging panic alert {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update alert status and log acknowledgment
            return Ok(new { message = "Alert acknowledged", acknowledgedBy = dto.AcknowledgedBy });
        }

        /// <summary>
        /// PUT: api/panicalerts/{id}/respond
        /// Add response to panic alert
        /// </summary>
        [HttpPut("{id}/respond", Name = "RespondToPanicAlert")]
        public IActionResult Respond(int id, [FromBody] RespondToAlertDto dto)
        {
            _logger.LogInformation($"Responding to panic alert {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add response to log
            return Ok(new { message = "Response logged" });
        }

        /// <summary>
        /// PUT: api/panicalerts/{id}/resolve
        /// Resolve a panic alert
        /// </summary>
        [HttpPut("{id}/resolve", Name = "ResolvePanicAlert")]
        public IActionResult Resolve(int id, [FromBody] ResolveAlertDto dto)
        {
            _logger.LogInformation($"Resolving panic alert {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update status to resolved, set resolution time and notes
            return Ok(new { message = "Alert resolved" });
        }

        /// <summary>
        /// DELETE: api/panicalerts/{id}
        /// Cancel/delete a panic alert (driver only if false alarm)
        /// </summary>
        [HttpDelete("{id}", Name = "CancelPanicAlert")]
        public IActionResult Cancel(int id)
        {
            _logger.LogInformation($"Cancelling panic alert {id}");

            // TODO: Only allow driver who triggered to cancel within X minutes
            return Ok(new { message = "Alert cancelled" });
        }

        /// <summary>
        /// GET: api/panicalerts/stats
        /// Get panic alert statistics
        /// </summary>
        [HttpGet("stats", Name = "GetPanicAlertStats")]
        public IActionResult GetStats()
        {
            _logger.LogInformation("Getting panic alert statistics");

            var stats = new
            {
                TotalAlerts = 45,
                ActiveAlerts = 2,
                ResolvedAlerts = 40,
                CancelledAlerts = 3,
                AverageResponseTimeMinutes = 8.5,
                AlertsByType = new
                {
                    Emergency = 15,
                    Threat = 12,
                    Breakdown = 8,
                    Accident = 6,
                    Medical = 4
                },
                AlertsByMonth = new
                {
                    ThisMonth = 12,
                    LastMonth = 8
                }
            };

            return Ok(stats);
        }
    }

    // Panic Alert Models
    public class PanicAlert
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string VehicleRegistration { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty; // Emergency, Threat, Breakdown, Accident, Medical
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Active, Acknowledged, Resolved, Cancelled
        public string Priority { get; set; } = string.Empty; // Critical, High, Medium
        public DateTime TriggeredAt { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? AcknowledgedBy { get; set; }
        public string? ResolvedBy { get; set; }
        public string? ResolutionNotes { get; set; }
        public bool IsActive { get; set; }
        public double? ResponseTime { get; set; }
        public List<string>? EmergencyContactsNotified { get; set; }
        public List<ResponseLogEntry>? ResponseLog { get; set; }
    }

    public class ResponseLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    // DTOs
    public class TriggerPanicAlertDto
    {
        [Required]
        public int DriverId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string Location { get; set; } = string.Empty;

        [Required]
        public string AlertType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    public class AcknowledgeAlertDto
    {
        [Required]
        public string AcknowledgedBy { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }

    public class RespondToAlertDto
    {
        [Required]
        public string Action { get; set; } = string.Empty;

        [Required]
        public string PerformedBy { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }

    public class ResolveAlertDto
    {
        [Required]
        public string ResolvedBy { get; set; } = string.Empty;

        [Required]
        public string ResolutionNotes { get; set; } = string.Empty;
    }
}
