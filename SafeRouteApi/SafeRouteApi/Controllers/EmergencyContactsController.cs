using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Controllers
{
    /// <summary>
    /// API Controller for emergency contacts
    /// Phase 4: Supporting Features
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmergencyContactsController : ControllerBase
    {
        private readonly ILogger<EmergencyContactsController> _logger;

        public EmergencyContactsController(ILogger<EmergencyContactsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: api/emergencycontacts
        /// Get all emergency contacts for current user
        /// </summary>
        [HttpGet(Name = "GetAllEmergencyContacts")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Getting all emergency contacts");

            var contacts = new List<EmergencyContact>
            {
                new EmergencyContact
                {
                    Id = 1,
                    DriverId = 1,
                    Name = "Mary Mbeki",
                    Relationship = "Spouse",
                    PhoneNumber = "0821234567",
                    Email = "mary.mbeki@example.com",
                    IsPrimary = true,
                    NotifyOnPanic = true,
                    NotifyOnRouteStart = true,
                    NotifyOnRouteEnd = false,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new EmergencyContact
                {
                    Id = 2,
                    DriverId = 1,
                    Name = "David Mbeki",
                    Relationship = "Brother",
                    PhoneNumber = "0829876543",
                    Email = "david.mbeki@example.com",
                    IsPrimary = false,
                    NotifyOnPanic = true,
                    NotifyOnRouteStart = false,
                    NotifyOnRouteEnd = false,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new EmergencyContact
                {
                    Id = 3,
                    DriverId = 1,
                    Name = "Company Dispatch",
                    Relationship = "Employer",
                    PhoneNumber = "0215551234",
                    Email = "dispatch@company.com",
                    IsPrimary = false,
                    NotifyOnPanic = true,
                    NotifyOnRouteStart = true,
                    NotifyOnRouteEnd = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-6)
                }
            };

            return Ok(contacts);
        }

        /// <summary>
        /// GET: api/emergencycontacts/{id}
        /// Get emergency contact by ID
        /// </summary>
        [HttpGet("{id}", Name = "GetEmergencyContactById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting emergency contact {id}");

            var contact = new EmergencyContact
            {
                Id = id,
                DriverId = 1,
                Name = "Mary Mbeki",
                Relationship = "Spouse",
                PhoneNumber = "0821234567",
                Email = "mary.mbeki@example.com",
                AlternatePhone = "0765432109",
                Address = "123 Main Street, Cape Town",
                IsPrimary = true,
                NotifyOnPanic = true,
                NotifyOnRouteStart = true,
                NotifyOnRouteEnd = false,
                NotifyOnIncident = true,
                IsActive = true,
                CreatedAt = DateTime.Now.AddMonths(-6),
                LastNotified = DateTime.Now.AddDays(-2)
            };

            return Ok(contact);
        }

        /// <summary>
        /// GET: api/emergencycontacts/driver/{driverId}
        /// Get emergency contacts for a specific driver
        /// </summary>
        [HttpGet("driver/{driverId}", Name = "GetEmergencyContactsByDriver")]
        public IActionResult GetByDriver(int driverId)
        {
            _logger.LogInformation($"Getting emergency contacts for driver {driverId}");

            var contacts = new List<EmergencyContact>
            {
                new EmergencyContact
                {
                    Id = 1,
                    DriverId = driverId,
                    Name = "Mary Mbeki",
                    Relationship = "Spouse",
                    PhoneNumber = "0821234567",
                    IsPrimary = true,
                    IsActive = true
                }
            };

            return Ok(contacts);
        }

        /// <summary>
        /// GET: api/emergencycontacts/primary
        /// Get primary emergency contact for current user
        /// </summary>
        [HttpGet("primary", Name = "GetPrimaryEmergencyContact")]
        public IActionResult GetPrimary()
        {
            _logger.LogInformation("Getting primary emergency contact");

            var contact = new EmergencyContact
            {
                Id = 1,
                DriverId = 1,
                Name = "Mary Mbeki",
                Relationship = "Spouse",
                PhoneNumber = "0821234567",
                Email = "mary.mbeki@example.com",
                IsPrimary = true,
                NotifyOnPanic = true,
                IsActive = true
            };

            return Ok(contact);
        }

        /// <summary>
        /// POST: api/emergencycontacts
        /// Add a new emergency contact
        /// </summary>
        [HttpPost(Name = "CreateEmergencyContact")]
        public IActionResult Create([FromBody] CreateEmergencyContactDto dto)
        {
            _logger.LogInformation($"Creating emergency contact: {dto.Name}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = new EmergencyContact
            {
                Id = 1,
                DriverId = dto.DriverId,
                Name = dto.Name,
                Relationship = dto.Relationship,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                AlternatePhone = dto.AlternatePhone,
                Address = dto.Address,
                IsPrimary = dto.IsPrimary,
                NotifyOnPanic = dto.NotifyOnPanic,
                NotifyOnRouteStart = dto.NotifyOnRouteStart,
                NotifyOnRouteEnd = dto.NotifyOnRouteEnd,
                NotifyOnIncident = dto.NotifyOnIncident,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            return CreatedAtRoute("GetEmergencyContactById", new { id = contact.Id }, contact);
        }

        /// <summary>
        /// PUT: api/emergencycontacts/{id}
        /// Update emergency contact information
        /// </summary>
        [HttpPut("{id}", Name = "UpdateEmergencyContact")]
        public IActionResult Update(int id, [FromBody] UpdateEmergencyContactDto dto)
        {
            _logger.LogInformation($"Updating emergency contact {id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Update in database
            return NoContent();
        }

        /// <summary>
        /// PUT: api/emergencycontacts/{id}/set-primary
        /// Set as primary emergency contact
        /// </summary>
        [HttpPut("{id}/set-primary", Name = "SetPrimaryContact")]
        public IActionResult SetPrimary(int id)
        {
            _logger.LogInformation($"Setting contact {id} as primary");

            // TODO: Unset other primary contacts and set this one as primary
            return Ok(new { message = "Primary contact updated" });
        }

        /// <summary>
        /// DELETE: api/emergencycontacts/{id}
        /// Delete an emergency contact
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteEmergencyContact")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting emergency contact {id}");

            // TODO: Delete from database or set IsActive to false
            return NoContent();
        }

        /// <summary>
        /// POST: api/emergencycontacts/notify
        /// Manually notify emergency contacts
        /// </summary>
        [HttpPost("notify", Name = "NotifyEmergencyContacts")]
        public IActionResult Notify([FromBody] NotifyContactsDto dto)
        {
            _logger.LogInformation($"Notifying emergency contacts for driver {dto.DriverId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Send SMS/Email to emergency contacts
            var notifiedContacts = new List<string> { "Mary Mbeki", "David Mbeki" };

            return Ok(new
            {
                message = "Emergency contacts notified",
                notifiedContacts,
                timestamp = DateTime.Now
            });
        }

        /// <summary>
        /// POST: api/emergencycontacts/{id}/test
        /// Send test notification to emergency contact
        /// </summary>
        [HttpPost("{id}/test", Name = "TestEmergencyContact")]
        public IActionResult TestNotification(int id)
        {
            _logger.LogInformation($"Sending test notification to contact {id}");

            // TODO: Send test SMS/Email
            return Ok(new { message = "Test notification sent" });
        }

        /// <summary>
        /// GET: api/emergencycontacts/stats
        /// Get emergency contact statistics
        /// </summary>
        [HttpGet("stats", Name = "GetEmergencyContactStats")]
        public IActionResult GetStats()
        {
            _logger.LogInformation("Getting emergency contact statistics");

            var stats = new
            {
                TotalContacts = 3,
                ActiveContacts = 3,
                PrimaryContacts = 1,
                ContactsWithPanicAlert = 2,
                ContactsWithRouteUpdates = 2,
                TotalNotificationsSent = 45,
                NotificationsThisMonth = 12,
                LastNotificationSent = DateTime.Now.AddDays(-2)
            };

            return Ok(stats);
        }
    }

    // Emergency Contact Models
    public class EmergencyContact
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty; // Spouse, Parent, Sibling, Friend, Employer, etc.
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AlternatePhone { get; set; }
        public string? Address { get; set; }
        public bool IsPrimary { get; set; }
        public bool NotifyOnPanic { get; set; }
        public bool NotifyOnRouteStart { get; set; }
        public bool NotifyOnRouteEnd { get; set; }
        public bool NotifyOnIncident { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastNotified { get; set; }
    }

    // DTOs
    public class CreateEmergencyContactDto
    {
        [Required]
        public int DriverId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Relationship { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? AlternatePhone { get; set; }

        public string? Address { get; set; }

        public bool IsPrimary { get; set; } = false;

        public bool NotifyOnPanic { get; set; } = true;

        public bool NotifyOnRouteStart { get; set; } = false;

        public bool NotifyOnRouteEnd { get; set; } = false;

        public bool NotifyOnIncident { get; set; } = false;
    }

    public class UpdateEmergencyContactDto
    {
        public string? Name { get; set; }
        public string? Relationship { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AlternatePhone { get; set; }
        public string? Address { get; set; }
        public bool? NotifyOnPanic { get; set; }
        public bool? NotifyOnRouteStart { get; set; }
        public bool? NotifyOnRouteEnd { get; set; }
        public bool? NotifyOnIncident { get; set; }
        public bool? IsActive { get; set; }
    }

    public class NotifyContactsDto
    {
        [Required]
        public int DriverId { get; set; }

        [Required]
        public string NotificationType { get; set; } = string.Empty; // Panic, RouteStart, RouteEnd, Incident

        [Required]
        public string Message { get; set; } = string.Empty;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Location { get; set; }
    }
}
