using Microsoft.AspNetCore.Mvc;
using SafeRouteApi.Models;
using RouteModel = SafeRouteApi.Models.Route;

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
        var routes = new List<RouteModel>
          {
  new RouteModel
       {
      Id = 1,
        DriverId = 1,
      StartLocation = "Cape Town City Centre",
    EndLocation = "Stellenbosch",
      StartLatitude = -33.9249,
           StartLongitude = 18.4241,
          EndLatitude = -33.9321,
      EndLongitude = 18.8602,
        StartTime = DateTime.Now.AddHours(-2),
        EndTime = DateTime.Now.AddHours(-1),
   Status = "Completed",
      Distance = 52.5,
    Duration = 45,
     SafetyScore = 4.5,
     RiskZonesEncountered = 2,
      CreatedAt = DateTime.Now.AddHours(-3)
       },
      new RouteModel
    {
    Id = 2,
    DriverId = 1,
   StartLocation = "Khayelitsha",
   EndLocation = "Mitchell's Plain",
          StartLatitude = -34.0290,
       StartLongitude = 18.6790,
      EndLatitude = -34.0515,
      EndLongitude = 18.6298,
    StartTime = DateTime.Now.AddMinutes(-30),
         Status = "InProgress",
          Distance = 12.3,
         RiskZonesEncountered = 1,
        CreatedAt = DateTime.Now.AddMinutes(-35)
       },
         new RouteModel
           {
        Id = 3,
  DriverId = 2,
        StartLocation = "Bellville",
  EndLocation = "Paarl",
      StartLatitude = -33.8961,
  StartLongitude = 18.6292,
EndLatitude = -33.7344,
  EndLongitude = 18.9645,
       StartTime = DateTime.Now.AddDays(1),
     Status = "Planned",
         Distance = 45.0,
    CreatedAt = DateTime.Now
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

     var route = new RouteModel
     {
       Id = id,
   DriverId = 1,
   StartLocation = "Cape Town",
     EndLocation = "Stellenbosch",
    StartLatitude = -33.9249,
   StartLongitude = 18.4241,
   EndLatitude = -33.9321,
      EndLongitude = 18.8602,
  StartTime = DateTime.Now.AddHours(-2),
 EndTime = DateTime.Now.AddHours(-1),
   Status = "Completed",
  Distance = 52.5,
      Duration = 45,
 SafetyScore = 4.5,
    RiskZonesEncountered = 2,
     CreatedAt = DateTime.Now.AddHours(-3)
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

      var routes = new List<RouteModel>
          {
         new RouteModel
   {
   Id = 1,
DriverId = driverId,
   StartLocation = "Cape Town City Centre",
     EndLocation = "Stellenbosch",
 StartLatitude = -33.9249,
          StartLongitude = 18.4241,
   EndLatitude = -33.9321,
    EndLongitude = 18.8602,
    StartTime = DateTime.Now.AddHours(-2),
   Status = "Completed",
  Distance = 52.5,
          SafetyScore = 4.5,
   CreatedAt = DateTime.Now.AddHours(-3)
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

          var routes = new List<RouteModel>
      {
 new RouteModel
      {
          Id = 2,
      DriverId = driverId,
  StartLocation = "Khayelitsha",
        EndLocation = "Mitchell's Plain",
 StartLatitude = -34.0290,
         StartLongitude = 18.6790,
     EndLatitude = -34.0515,
   EndLongitude = 18.6298,
   StartTime = DateTime.Now.AddMinutes(-30),
   Status = "InProgress",
 RiskZonesEncountered = 1,
   CreatedAt = DateTime.Now.AddMinutes(-35)
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

   var createdRoute = new RouteModel
  {
     Id = 999, // Would be generated by database
     DriverId = dto.DriverId,
        StartLocation = dto.StartLocation,
     EndLocation = dto.EndLocation,
        StartLatitude = dto.StartLatitude,
            StartLongitude = dto.StartLongitude,
         EndLatitude = dto.EndLatitude,
         EndLongitude = dto.EndLongitude,
      StartTime = dto.StartTime ?? DateTime.Now,
  Status = "Planned",
    CreatedAt = DateTime.Now
   };

    return CreatedAtRoute(
       "GetRouteById",
     new { id = createdRoute.Id },
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
     _logger.LogInformation($"Updating status for route {id} to {dto.Status}");

       if (!ModelState.IsValid)
        {
      return BadRequest(ModelState);
  }

            // TODO: Update in database
   var updatedRoute = new RouteModel
 {
                Id = id,
           DriverId = 1,
      StartLocation = "Cape Town",
      EndLocation = "Stellenbosch",
          StartLatitude = -33.9249,
      StartLongitude = 18.4241,
       EndLatitude = -33.9321,
         EndLongitude = 18.8602,
     StartTime = DateTime.Now.AddHours(-1),
        EndTime = dto.EndTime,
 Status = dto.Status,
     Notes = dto.Notes,
    UpdatedAt = DateTime.Now
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
}
