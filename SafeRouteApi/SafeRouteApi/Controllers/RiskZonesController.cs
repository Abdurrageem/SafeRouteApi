using Microsoft.AspNetCore.Mvc;
using SafeRouteApi.Models;

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

          // Mock data - replace with database call
    var riskZones = new List<RiskZone>
        {
           new RiskZone
   {
      Id = 1,
              Name = "Nyanga",
             Location = "Nyanga, Cape Town",
Latitude = -34.0276,
         Longitude = 18.5881,
         Radius = 2.5,
         RiskLevel = "High",
           RiskType = "Crime",
               Description = "High crime rate area with frequent incidents",
     IncidentCount = 45,
             LastIncidentDate = DateTime.Now.AddDays(-2),
                IsActive = true,
         CreatedAt = DateTime.Now.AddMonths(-6)
         },
        new RiskZone
    {
           Id = 2,
   Name = "Khayelitsha",
     Location = "Khayelitsha, Cape Town",
         Latitude = -34.0290,
  Longitude = 18.6790,
            Radius = 3.0,
       RiskLevel = "High",
  RiskType = "Crime",
        Description = "High-risk area with multiple reported incidents",
     IncidentCount = 38,
         LastIncidentDate = DateTime.Now.AddDays(-1),
  IsActive = true,
             CreatedAt = DateTime.Now.AddMonths(-6)
    },
                new RiskZone
    {
         Id = 3,
           Name = "Philippi",
          Location = "Philippi, Cape Town",
  Latitude = -34.0022,
  Longitude = 18.5946,
     Radius = 2.0,
       RiskLevel = "Medium",
        RiskType = "Crime",
          Description = "Moderate risk area, exercise caution",
             IncidentCount = 22,
         LastIncidentDate = DateTime.Now.AddDays(-5),
           IsActive = true,
        CreatedAt = DateTime.Now.AddMonths(-4)
                },
  new RiskZone
           {
       Id = 4,
     Name = "Chapman's Peak Drive",
           Location = "Chapman's Peak, Cape Town",
   Latitude = -34.0928,
   Longitude = 18.3551,
           Radius = 1.5,
    RiskLevel = "Medium",
             RiskType = "Weather",
 Description = "Road closures during high winds and heavy rain",
 IncidentCount = 12,
          LastIncidentDate = DateTime.Now.AddDays(-10),
             IsActive = true,
            CreatedAt = DateTime.Now.AddMonths(-12)
  },
                new RiskZone
      {
     Id = 5,
   Name = "N1 - N2 Interchange",
      Location = "N1/N2 Interchange, Cape Town",
      Latitude = -33.9416,
        Longitude = 18.5041,
      Radius = 1.0,
   RiskLevel = "Low",
RiskType = "Traffic",
              Description = "Heavy traffic during peak hours",
         IncidentCount = 8,
          LastIncidentDate = DateTime.Now.AddDays(-3),
  IsActive = true,
           CreatedAt = DateTime.Now.AddMonths(-8)
                }
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

         var riskZone = new RiskZone
            {
      Id = id,
         Name = "Nyanga",
             Location = "Nyanga, Cape Town",
        Latitude = -34.0276,
         Longitude = 18.5881,
    Radius = 2.5,
    RiskLevel = "High",
      RiskType = "Crime",
          Description = "High crime rate area with frequent incidents",
  IncidentCount = 45,
          LastIncidentDate = DateTime.Now.AddDays(-2),
        IsActive = true,
         CreatedAt = DateTime.Now.AddMonths(-6)
          };

            return Ok(riskZone);
        }

        /// <summary>
        /// GET: api/riskzones/active
        /// Get all active risk zones
   /// </summary>
        [HttpGet("active", Name = "GetActiveRiskZones")]
      public IActionResult GetActive()
        {
            _logger.LogInformation("Getting active risk zones");

       var riskZones = new List<RiskZone>
            {
                new RiskZone
  {
          Id = 1,
        Name = "Nyanga",
            Location = "Nyanga, Cape Town",
  Latitude = -34.0276,
                Longitude = 18.5881,
                    Radius = 2.5,
   RiskLevel = "High",
          RiskType = "Crime",
        Description = "High crime rate area",
         IncidentCount = 45,
             IsActive = true
       }
          };

 return Ok(riskZones);
        }

        /// <summary>
        /// GET: api/riskzones/level/{level}
  /// Get risk zones by risk level (Low, Medium, High, Critical)
        /// </summary>
        [HttpGet("level/{level}", Name = "GetRiskZonesByLevel")]
        public IActionResult GetByLevel(string level)
        {
  _logger.LogInformation($"Getting risk zones with level: {level}");

       var riskZones = new List<RiskZone>
 {
            new RiskZone
     {
      Id = 1,
        Name = "Nyanga",
           Location = "Nyanga, Cape Town",
      Latitude = -34.0276,
                    Longitude = 18.5881,
              Radius = 2.5,
      RiskLevel = level,
        RiskType = "Crime",
   Description = $"{level} risk area",
        IsActive = true
       }
  };

return Ok(riskZones);
   }

        /// <summary>
        /// GET: api/riskzones/type/{type}
        /// Get risk zones by type (Crime, Accident, Weather, Traffic, Other)
        /// </summary>
        [HttpGet("type/{type}", Name = "GetRiskZonesByType")]
        public IActionResult GetByType(string type)
    {
            _logger.LogInformation($"Getting risk zones with type: {type}");

         var riskZones = new List<RiskZone>
            {
      new RiskZone
        {
    Id = 1,
            Name = "Sample Zone",
                 Location = "Cape Town",
        Latitude = -33.9249,
    Longitude = 18.4241,
              Radius = 2.0,
     RiskLevel = "Medium",
       RiskType = type,
             Description = $"{type} related risk zone",
    IsActive = true
         }
     };

return Ok(riskZones);
     }

        /// <summary>
     /// POST: api/riskzones/check
        /// Check if a location is within any risk zones
    /// </summary>
      [HttpPost("check", Name = "CheckLocation")]
        public IActionResult CheckLocation([FromBody] CheckLocationDto dto)
        {
            _logger.LogInformation($"Checking location: {dto.Latitude}, {dto.Longitude}");

            if (!ModelState.IsValid)
     {
    return BadRequest(ModelState);
            }

            // Mock response - implement actual distance calculation
          var nearbyRiskZones = new List<object>
            {
    new
       {
   RiskZone = new RiskZone
       {
    Id = 1,
           Name = "Nyanga",
         Location = "Nyanga, Cape Town",
            Latitude = -34.0276,
        Longitude = 18.5881,
          RiskLevel = "High",
   RiskType = "Crime",
      Description = "High crime rate area"
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

            var createdRiskZone = new RiskZone
{
                Id = 999, // Would be generated by database
     Name = dto.Name,
 Location = dto.Location,
      Latitude = dto.Latitude,
     Longitude = dto.Longitude,
     Radius = dto.Radius,
              RiskLevel = dto.RiskLevel,
            RiskType = dto.RiskType,
      Description = dto.Description,
        IncidentCount = 0,
   IsActive = true,
     CreatedAt = DateTime.Now
  };

     return CreatedAtRoute(
 "GetRiskZoneById",
        new { id = createdRiskZone.Id },
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
     [HttpPut("{id}/incident", Name = "ReportIncident")]
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

    // TODO: Set IsActive to false in database
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

            // TODO: Set IsActive to true in database
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

      // Mock data - implement actual distance calculation
   var nearbyRiskZones = new List<object>
            {
              new
         {
    RiskZone = new RiskZone
          {
      Id = 1,
    Name = "Nyanga",
        Location = "Nyanga, Cape Town",
      Latitude = -34.0276,
           Longitude = 18.5881,
   RiskLevel = "High",
               RiskType = "Crime",
            IsActive = true
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
      CriticalRiskZones = 7,
  TotalIncidents = 456,
          RiskZonesByType = new
     {
       Crime = 25,
      Accident = 8,
 Weather = 6,
           Traffic = 4,
     Other = 2
  }
         };

   return Ok(stats);
     }
    }
}
