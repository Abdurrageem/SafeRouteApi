using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.DTOs.Routes;

public class CreateRouteDto
{
    [Required] public int DriverId { get; set; }
    [Required] public string Start { get; set; } = string.Empty;
    [Required] public string End { get; set; } = string.Empty;
    public DateTime? StartTime { get; set; }
}