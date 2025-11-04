using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteApi.Models
{
    public class Routes
    {
        [Key]
        public int RouteId { get; set; }

        // Foreign key for drivers
        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public virtual Drivers? Driver { get; set; }

        [Required]
        public string Start { get; set; } = string.Empty;

        [Required]
        public string End { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string RouteStatus { get; set; } = string.Empty;

        public float EstimatedDistance { get; set; }

        public float ActualDistance { get; set; }

        [Required]
        [MaxLength(20)]
        public string RiskLevel { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<PanicAlerts> PanicAlerts { get; set; } = new List<PanicAlerts>();
        public virtual ICollection<Incidents> Incidents { get; set; } = new List<Incidents>();
        public virtual ICollection<CameraRecordings> CameraRecordings { get; set; } = new List<CameraRecordings>();
        public virtual ICollection<ThreatDetections> ThreatDetections { get; set; } = new List<ThreatDetections>();
    }
}
