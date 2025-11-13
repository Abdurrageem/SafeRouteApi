using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Data;
using SafeRoute.Models;
using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Controllers
{
    /// <summary>
    /// API Controller for emergency contacts backed by EF Core
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmergencyContactsController : ControllerBase
    {
        private readonly ILogger<EmergencyContactsController> _logger;
        private readonly SafeRouteDbContext _db;

        public EmergencyContactsController(ILogger<EmergencyContactsController> logger, SafeRouteDbContext db)
        { _logger = logger; _db = db; }

        /// <summary>
        /// GET: api/emergencycontacts
        /// Get all emergency contacts for current user
        /// </summary>
        [HttpGet(Name = "GetAllEmergencyContacts")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all emergency contacts");

            var contacts = await _db.EmergencyContacts.AsNoTracking().ToListAsync();
            return Ok(contacts);
        }

        /// <summary>
        /// GET: api/emergencycontacts/{id}
        /// Get emergency contact by ID
        /// </summary>
        [HttpGet("{id:int}", Name = "GetEmergencyContactById")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Getting emergency contact {id}");

            var contact = await _db.EmergencyContacts.AsNoTracking().FirstOrDefaultAsync(c => c.ContactId == id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        /// <summary>
        /// GET: api/emergencycontacts/driver/{driverId}
        /// Get emergency contacts for a specific driver
        /// </summary>
        [HttpGet("driver/{driverId:int}", Name = "GetEmergencyContactsByDriver")]
        public async Task<IActionResult> GetByDriver(int driverId)
        {
            _logger.LogInformation($"Getting emergency contacts for driver {driverId}");

            var contacts = await _db.EmergencyContacts.Where(c => c.DriverId == driverId).AsNoTracking().ToListAsync();
            return Ok(contacts);
        }

        /// <summary>
        /// POST: api/emergencycontacts
        /// Add a new emergency contact
        /// </summary>
        [HttpPost(Name = "CreateEmergencyContact")]
        public async Task<IActionResult> Create([FromBody] CreateEmergencyContactDto dto)
        {
            _logger.LogInformation($"Creating emergency contact: {dto.Name}");

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var contact = new EmergencyContacts
            {
                DriverId = dto.DriverId,
                Name = dto.Name,
                Surname = dto.Surname,
                Relationship = dto.Relationship,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };
            _db.EmergencyContacts.Add(contact);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = contact.ContactId }, contact);
        }

        /// <summary>
        /// PUT: api/emergencycontacts/{id}
        /// Update emergency contact information
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdateEmergencyContact")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmergencyContactDto dto)
        {
            _logger.LogInformation($"Updating emergency contact {id}");

            var contact = await _db.EmergencyContacts.FirstOrDefaultAsync(c => c.ContactId == id);
            if (contact == null) return NotFound();

            if (dto.Name != null) contact.Name = dto.Name;
            if (dto.Surname != null) contact.Surname = dto.Surname;
            if (dto.Relationship != null) contact.Relationship = dto.Relationship;
            if (dto.PhoneNumber != null) contact.PhoneNumber = dto.PhoneNumber;
            if (dto.Email != null) contact.Email = dto.Email;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/emergencycontacts/{id}
        /// Delete an emergency contact
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeleteEmergencyContact")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting emergency contact {id}");

            var contact = await _db.EmergencyContacts.FirstOrDefaultAsync(c => c.ContactId == id);
            if (contact == null) return NotFound();
            _db.EmergencyContacts.Remove(contact);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }

    // DTOs
    public class CreateEmergencyContactDto
    {
        [Required]
        public int DriverId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public string Relationship { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateEmergencyContactDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Relationship { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
