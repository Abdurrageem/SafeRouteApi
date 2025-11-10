using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteSharedLib.Models;

public class Incidents
{
    [Key] public int IncidentId { get; set; }
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))] public virtual Drivers? Driver { get; set; }
    public int? AlertId { get; set; }
    [ForeignKey(nameof(AlertId))] public virtual PanicAlerts? PanicAlert { get; set; }
    public int RouteId { get; set; }
    [ForeignKey(nameof(RouteId))] public virtual Routes? Route { get; set; }
    public int ZoneId { get; set; }
    [ForeignKey(nameof(ZoneId))] public virtual RiskZones? RiskZone { get; set; }

    [Required, MaxLength(100)] public string Type { get; set; } = string.Empty;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    [Required, MaxLength(20)] public string Severity { get; set; } = string.Empty;
    public DateTime IncidentTime { get; set; }
    [Required, MaxLength(50)] public string Status { get; set; } = string.Empty;

    public virtual ICollection<IncidentResponses> IncidentResponses { get; set; } = new List<IncidentResponses>();
}
