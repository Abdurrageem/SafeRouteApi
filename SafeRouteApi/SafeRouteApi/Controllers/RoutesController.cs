using Microsoft.AspNetCore.Mvc;
using SafeRoute.DTOs.Common;
using SafeRoute.DTOs.Routes;
using SafeRoute.Services.Interfaces;

namespace SafeRoute.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly ILogger<RoutesController> _logger;
    private readonly IRouteService _routes;

    public RoutesController(ILogger<RoutesController> logger, IRouteService routes)
    { _logger = logger; _routes = routes; }

    /// <summary>
    /// GET: api/routes
    /// Get all routes
    /// </summary>
    [HttpGet(Name = "GetAllRoutes")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams page)
    {
        var (items, total) = await _routes.GetRoutesAsync(page);
        return Ok(new { total, items });
    }

    /// <summary>
    /// GET: api/routes/{id}
    /// Get route by ID
    /// </summary>
    [HttpGet("{id}", Name = "GetRouteById")]
    public async Task<ActionResult<RouteDetailDto>> GetById(int id)
    {
        var route = await _routes.GetRouteAsync(id);
        if (route == null) return NotFound();
        return Ok(route);
    }

    /// <summary>
    /// GET: api/routes/driver/{driverId}
    /// Get all routes for a specific driver
    /// </summary>
    [HttpGet("driver/{driverId}", Name = "GetRoutesByDriver")]
    public async Task<IActionResult> GetByDriver(int driverId, [FromQuery] PaginationParams page)
    {
        var (items, total) = await _routes.GetRoutesByDriverAsync(driverId, page);
        return Ok(new { total, items });
    }

    /// <summary>
    /// POST: api/routes
    /// Create a new route
    /// </summary>
    [HttpPost(Name = "CreateRoute")]
    public async Task<IActionResult> Create([FromBody] CreateRouteDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await _routes.CreateRouteAsync(dto);
        return CreatedAtRoute("GetRouteById", new { id }, new { id });
    }

    /// <summary>
    /// PUT: api/routes/{id}/status
    /// Update route status
    /// </summary>
    [HttpPut("{id}/status", Name = "UpdateRouteStatus")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRouteStatusDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var ok = await _routes.updateStatusAsync(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// DELETE: api/routes/{id}
    /// Delete a route
    /// </summary>
    [HttpDelete("{id}", Name = "DeleteRoute")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _routes.DeleteRouteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
