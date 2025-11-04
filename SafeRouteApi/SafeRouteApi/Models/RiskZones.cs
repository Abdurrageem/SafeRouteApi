using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Models
{
    public class RiskZones
    {
        [Key]
        public int ZoneId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public bool Incident { get; set; }

        public int IncidentCount { get; set; }

        public DateTime? LastIncident { get; set; }

        [Required]
        [MaxLength(20)]
        public string RiskLevel { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Incidents> Incidents { get; set; } = new List<Incidents>();
    }
}
