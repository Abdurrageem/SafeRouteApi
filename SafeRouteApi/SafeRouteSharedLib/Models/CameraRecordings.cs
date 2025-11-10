using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteSharedLib.Models;

public class CameraRecordings
{
    [Key] public int RecordingId { get; set; }
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))] public virtual Drivers? Driver { get; set; }
    public int RouteId { get; set; }
    [ForeignKey(nameof(RouteId))] public virtual Routes? Route { get; set; }
    public int? ThreatId { get; set; }
    [ForeignKey(nameof(ThreatId))] public virtual ThreatDetections? ThreatDetected { get; set; }

    [Required, MaxLength(500)] public string FilePath { get; set; } = string.Empty;
    public float FileSize { get; set; }
    [Required, MaxLength(50)] public string Camera { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string Quality { get; set; } = string.Empty;
    public bool Evidence { get; set; }
}
