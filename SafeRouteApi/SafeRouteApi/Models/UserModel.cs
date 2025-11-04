using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Models
{
    /// <summary>
    /// User model for authentication
  /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
      [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
  public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Driver"; // Driver, Admin, Dispatcher

        public bool IsActive { get; set; } = true;

      public bool EmailConfirmed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

     // Navigation property
        public Driver? Driver { get; set; }
    }

    /// <summary>
    /// DTO for user registration
    /// </summary>
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

    /// <summary>
    /// DTO for user login
    /// </summary>
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for authentication
    /// </summary>
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

    /// <summary>
    /// DTO for token refresh
    /// </summary>
    public class RefreshTokenDto
    {
        [Required]
   public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for password change
    /// </summary>
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
      [MinLength(6)]
  public string NewPassword { get; set; } = string.Empty;
    }
}
