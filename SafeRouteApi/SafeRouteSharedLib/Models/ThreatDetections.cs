using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteSharedLib.Models;

public class ThreatDetections
{
    [Key] public int ThreatId { get; set; }
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))] public virtual Drivers? Driver { get; set; }
    public int? RouteId { get; set; }
    [ForeignKey(nameof(RouteId))] public virtual Routes? Route { get; set; }

    public double ConfidenceScore { get; set; }
    [MaxLength(1000)] public string ManualReview { get; set; } = string.Empty;
    public bool ConfirmedThreat { get; set; }
    public DateTime DetectionTime { get; set; }
    [Required, MaxLength(100)] public string ThreatType { get; set; } = string.Empty;
    public byte[] CameraData { get; set; } = Array.Empty<byte>();
    public float Latitude { get; set; }
    public float Longitude { get; set; }

    public virtual ICollection<CameraRecordings> CameraRecordings { get; set; } = new List<CameraRecordings>();
}
