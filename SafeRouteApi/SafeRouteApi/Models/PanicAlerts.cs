using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteApi.Models
{
    public class PanicAlerts
    {
        [Key]
        public int AlertId { get; set; }

        // Foreign key for drivers
        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public virtual Drivers? Driver { get; set; }

        // Foreign key for routes
        public int RouteId { get; set; }

        [ForeignKey(nameof(RouteId))]
        public virtual Routes? Route { get; set; }

        public DateTime Alert { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string AlertType { get; set; } = string.Empty;

        public DateTime? ResolvedTime { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Incidents> Incidents { get; set; } = new List<Incidents>();
    }
}
