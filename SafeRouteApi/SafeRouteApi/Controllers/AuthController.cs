using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Controllers
{
    /// <summary>
    /// API Controller for authentication - Login required first
    /// Phase 1: Core Functionality
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST: api/auth/register
        /// Register a new user account
        /// </summary>
        [HttpPost("register", Name = "Register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            _logger.LogInformation($"Registering new user: {dto.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResponse = new AuthResponse
            {
                UserId = 1,
                DriverId = 1,
                Email = dto.Email,
                Name = $"{dto.Name} {dto.Surname}",
                Role = "Driver",
                AccessToken = "mock_access_token_" + Guid.NewGuid().ToString(),
                RefreshToken = "mock_refresh_token_" + Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return CreatedAtRoute("GetDriverById", new { id = authResponse.DriverId }, authResponse);
        }

        /// <summary>
        /// POST: api/auth/login
        /// Login with email and password
        /// </summary>
        [HttpPost("login", Name = "Login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation($"User login attempt: {dto.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResponse = new AuthResponse
            {
                UserId = 1,
                DriverId = 1,
                Email = dto.Email,
                Name = "John Mbeki",
                Role = "Driver",
                AccessToken = "mock_access_token_" + Guid.NewGuid().ToString(),
                RefreshToken = "mock_refresh_token_" + Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return Ok(authResponse);
        }

        /// <summary>
        /// POST: api/auth/refresh
        /// Refresh access token using refresh token
        /// </summary>
        [HttpPost("refresh", Name = "RefreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenDto dto)
        {
            _logger.LogInformation("Refreshing access token");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResponse = new AuthResponse
            {
                UserId = 1,
                DriverId = 1,
                Email = "john.mbeki@example.com",
                Name = "John Mbeki",
                Role = "Driver",
                AccessToken = "mock_access_token_" + Guid.NewGuid().ToString(),
                RefreshToken = "mock_refresh_token_" + Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return Ok(authResponse);
        }

        /// <summary>
        /// POST: api/auth/logout
        /// Logout current user
        /// </summary>
        [HttpPost("logout", Name = "Logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation("User logout");
            return Ok(new { message = "Logged out successfully" });
        }

        /// <summary>
        /// POST: api/auth/change-password
        /// Change user password
        /// </summary>
        [HttpPost("change-password", Name = "ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto dto)
        {
            _logger.LogInformation("User password change request");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { message = "Password changed successfully" });
        }

        /// <summary>
        /// GET: api/auth/me
        /// Get current authenticated user info
        /// </summary>
        [HttpGet("me", Name = "GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            _logger.LogInformation("Getting current user info");

            var user = new
            {
                UserId = 1,
                DriverId = 1,
                Email = "john.mbeki@example.com",
                Name = "John Mbeki",
                Role = "Driver",
                IsActive = true,
                EmailConfirmed = true
            };

            return Ok(user);
        }

        /// <summary>
        /// POST: api/auth/forgot-password
        /// Request password reset email
        /// </summary>
        [HttpPost("forgot-password", Name = "ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            _logger.LogInformation($"Password reset requested for: {dto.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { message = "Password reset email sent" });
        }

        /// <summary>
        /// POST: api/auth/reset-password
        /// Reset password with token from email
        /// </summary>
        [HttpPost("reset-password", Name = "ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto dto)
        {
            _logger.LogInformation("Password reset with token");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { message = "Password reset successfully" });
        }
    }

    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required]
        public string VehicleRegistration { get; set; } = string.Empty;

        public string VehicleModel { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public int UserId { get; set; }
        public int DriverId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class RefreshTokenDto
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
