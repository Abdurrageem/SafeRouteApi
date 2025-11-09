namespace SafeRouteApi.DTOs.Drivers;

public class DriverListDto
{
    public int DriverId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string VehicleRegistration { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
}
