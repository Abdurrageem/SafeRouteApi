using Microsoft.AspNetCore.Mvc;
using SafeRouteApi.Services.Interfaces;
using SafeRouteApi.DTOs.Common;
using SafeRouteApi.DTOs.Drivers;

namespace SafeRouteApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly IDriverService _drivers;

    public DriversController(ILogger<DriversController> logger, IDriverService drivers)
    { _logger = logger; _drivers = drivers; }

    /// <summary>
    /// GET: api/drivers
    /// </summary>
    [HttpGet(Name = "GetAllDrivers")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams page)
    {
        _logger.LogInformation("GetAll drivers page {Page} size {Size}", page.Page, page.PageSize);
        var (items, total) = await _drivers.GetDriversAsync(page);
        return Ok(new { total, items });
    }

    /// <summary>
    /// GET: api/drivers/{id}
    /// </summary>
    [HttpGet("{id}", Name = "GetDriverById")]
    public async Task<ActionResult<DriverDetailDto>> GetById(int id)
    {
        var driver = await _drivers.GetDriverAsync(id);
        if (driver == null) return NotFound();
        return Ok(driver);
    }

    /// <summary>
    /// POST: api/drivers
    /// </summary>
    [HttpPost(Name = "CreateDriver")]
    public async Task<IActionResult> Create([FromBody] CreateDriverDto dto)
    {
        var id = await _drivers.CreateDriverAsync(dto.Name, dto.Surname, dto.LicenseNumber, dto.VehicleRegistration, dto.VehicleModel, dto.UserId);
        return CreatedAtRoute("GetDriverById", new { id }, new { id });
    }

    /// <summary>
    /// PUT: api/drivers/{id}
    /// </summary>
    [HttpPut("{id}", Name = "UpdateDriver")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDriverDto dto)
    {
        var ok = await _drivers.UpdateDriverAsync(id, dto.Name, dto.Surname, dto.VehicleRegistration, dto.VehicleModel);
        if (!ok) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// DELETE: api/drivers/{id}
    /// </summary>
    [HttpDelete("{id}", Name = "DeleteDriver")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _drivers.DeleteDriverAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
