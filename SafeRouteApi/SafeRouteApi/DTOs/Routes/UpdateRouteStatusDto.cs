using System.ComponentModel.DataAnnotations;

namespace SafeRoute.DTOs.Routes;

public class UpdateRouteStatusDto
{
    [Required] public string RouteStatus { get; set; } = string.Empty;
    public DateTime? EndTime { get; set; }
}
