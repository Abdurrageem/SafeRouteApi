using System.ComponentModel.DataAnnotations;

namespace SafeRoute.DTOs.Drivers;

public class CreateDriverDto
{
    [Required] public int UserId { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Surname { get; set; } = string.Empty;
    [Required] public string LicenseNumber { get; set; } = string.Empty;
    [Required] public string VehicleRegistration { get; set; } = string.Empty;
    [Required] public string VehicleModel { get; set; } = string.Empty;
}
