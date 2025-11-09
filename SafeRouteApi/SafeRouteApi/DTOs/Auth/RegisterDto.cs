using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.DTOs.Auth;

public class RegisterDto
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Surname { get; set; } = string.Empty;
    [Required] public string LicenseNumber { get; set; } = string.Empty;
    [Required] public string VehicleRegistration { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
}
