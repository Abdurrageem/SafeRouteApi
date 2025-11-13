using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SafeRoute.Data;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Models;
using Microsoft.AspNetCore.Authorization;
using SafeRoute.DTOs.Auth;

namespace SafeRoute.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly SafeRouteDbContext _db;
    private readonly ITokenService _tokens;
    private readonly IPasswordHasher _hasher;

    public AuthController(ILogger<AuthController> logger, SafeRouteDbContext db, ITokenService tokens, IPasswordHasher hasher)
    { _logger = logger; _db = db; _tokens = tokens; _hasher = hasher; }

    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email)) return Conflict(new { message = "Email already registered" });

        var company = await _db.Companies.FirstOrDefaultAsync();
        if (company == null)
        {
            company = new Companies { Name = "Default Co", Address = "", CompanyRegistration = "REG-1", Email = "default@co.com", ContactNumber = "000" };
            _db.Companies.Add(company);
            await _db.SaveChangesAsync();
        }

        var user = new Users { CompanyId = company.CompanyId, Email = dto.Email, Password = _hasher.Hash(dto.Password), Role = "Driver", CreatedAt = DateTime.UtcNow, UpdateAt = DateTime.UtcNow };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var driver = new Drivers { UserId = user.UserId, Name = dto.Name, Surname = dto.Surname, LicenseNumber = dto.LicenseNumber, VehicleRegistration = dto.VehicleRegistration, VehicleModel = dto.VehicleModel };
        _db.Drivers.Add(driver);
        await _db.SaveChangesAsync();

        var token = _tokens.CreateToken(user.UserId, user.Role, user.Email);
        _logger.LogInformation("User registered {UserId}", user.UserId);
        return Ok(new { user.UserId, driver.DriverId, token });
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _db.Users.Include(u => u.Driver).FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !_hasher.Verify(user.Password, dto.Password)) return Unauthorized(new { message = "Invalid credentials" });
        var token = _tokens.CreateToken(user.UserId, user.Role, user.Email);
        _logger.LogInformation("User login {UserId}", user.UserId);
        return Ok(new { user.UserId, user.Email, user.Role, DriverId = user.Driver?.DriverId ?? 0, token });
    }

    [HttpGet("me"), Authorize]
    public async Task<IActionResult> Me()
    {
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (idClaim == null || !int.TryParse(idClaim, out var userId)) return Unauthorized();
        var user = await _db.Users.Include(u => u.Driver).FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return NotFound();
        return Ok(new { user.UserId, user.Email, user.Role, Driver = user.Driver == null ? null : new { user.Driver.DriverId, user.Driver.Name } });
    }

    public class ChangePasswordDto { [Required] public string CurrentPassword { get; set; } = string.Empty; [Required, MinLength(6)] public string NewPassword { get; set; } = string.Empty; }

    [HttpPost("change-password"), Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (idClaim == null || !int.TryParse(idClaim, out var userId)) return Unauthorized();
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();
        if (!_hasher.Verify(user.Password, dto.CurrentPassword)) return Unauthorized(new { message = "Current password invalid" });
        user.Password = _hasher.Hash(dto.NewPassword);
        user.UpdateAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Password changed {UserId}", userId);
        return NoContent();
    }
}
