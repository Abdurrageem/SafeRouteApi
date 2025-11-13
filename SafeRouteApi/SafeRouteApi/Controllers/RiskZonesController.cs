using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Data;
using SafeRoute.Models;

namespace SafeRoute.Controllers
{
    /// <summary>
    /// API Controller for managing risk zones (EF-backed)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RiskZonesController : ControllerBase
    {
        private readonly ILogger<RiskZonesController> _logger;
        private readonly SafeRouteDbContext _db;

        public RiskZonesController(ILogger<RiskZonesController> logger, SafeRouteDbContext db)
        { _logger = logger; _db = db; }

        /// <summary>
        /// GET: api/riskzones
        /// Get all risk zones
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var zones = await _db.RiskZones.AsNoTracking().ToListAsync();
            return Ok(zones);
        }

        /// <summary>
        /// GET: api/riskzones/{id}
        /// Get risk zone by ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var zone = await _db.RiskZones.AsNoTracking().FirstOrDefaultAsync(z => z.ZoneId == id);
            if (zone == null) return NotFound();
            return Ok(zone);
        }

        /// <summary>
        /// POST: api/riskzones
        /// Create a new risk zone
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRiskZoneDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var zone = new RiskZones
            {
                Name = dto.Name,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                RiskLevel = dto.RiskLevel
            };
            _db.RiskZones.Add(zone);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = zone.ZoneId }, zone);
        }

        /// <summary>
        /// PUT: api/riskzones/{id}
        /// Update risk zone information
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRiskZoneDto dto)
        {
            var zone = await _db.RiskZones.FirstOrDefaultAsync(z => z.ZoneId == id);
            if (zone == null) return NotFound();
            if (dto.RiskLevel != null) zone.RiskLevel = dto.RiskLevel;
            if (dto.IncidentCount.HasValue) zone.IncidentCount = dto.IncidentCount.Value;
            if (dto.Description != null) { /* not modeled */ }
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/riskzones/{id}
        /// Delete a risk zone
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var zone = await _db.RiskZones.FirstOrDefaultAsync(z => z.ZoneId == id);
            if (zone == null) return NotFound();
            _db.RiskZones.Remove(zone);
            await _db.SaveChangesAsync();
            return NoContent();
        }
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
    }
}
