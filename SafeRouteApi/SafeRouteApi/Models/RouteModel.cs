using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Models
{
    /// <summary>
    /// Route model for tracking driver routes
    /// </summary>
    public class Route
    {
        [Key]
        public int Id { get; set; }

      [Required]
        public int DriverId { get; set; }

        [Required]
    public string StartLocation { get; set; } = string.Empty;

        [Required]
        public string EndLocation { get; set; } = string.Empty;

        public double StartLatitude { get; set; }

        public double StartLongitude { get; set; }

        public double EndLatitude { get; set; }

        public double EndLongitude { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        public string Status { get; set; } = "Planned"; // Planned, InProgress, Completed, Cancelled

        public double? Distance { get; set; } // Distance in kilometers

        public int? Duration { get; set; } // Duration in minutes

    public double? SafetyScore { get; set; }

        public int RiskZonesEncountered { get; set; } = 0;

      public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating a new route
    /// </summary>
    public class CreateRouteDto
    {
      [Required]
        public int DriverId { get; set; }

        [Required]
        public string StartLocation { get; set; } = string.Empty;

    [Required]
        public string EndLocation { get; set; } = string.Empty;

  public double StartLatitude { get; set; }

   public double StartLongitude { get; set; }

        public double EndLatitude { get; set; }

        public double EndLongitude { get; set; }

        public DateTime? StartTime { get; set; }
    }

    /// <summary>
/// DTO for updating route status
    /// </summary>
    public class UpdateRouteStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty; // InProgress, Completed, Cancelled

    public DateTime? EndTime { get; set; }

        public string? Notes { get; set; }
    }
}
