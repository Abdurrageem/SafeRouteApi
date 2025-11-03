using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Models
{
    /// <summary>
    /// Risk Zone model for tracking high-risk areas
    /// </summary>
    public class RiskZone
    {
        [Key]
      public int Id { get; set; }

     [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

     [Required]
        public double Latitude { get; set; }

        [Required]
     public double Longitude { get; set; }

        public double Radius { get; set; } = 1.0; // Radius in kilometers

        [Required]
        public string RiskLevel { get; set; } = "Medium"; // Low, Medium, High, Critical

        public string RiskType { get; set; } = "Crime"; // Crime, Accident, Weather, Traffic, Other

      public string Description { get; set; } = string.Empty;

        public int IncidentCount { get; set; } = 0;

    public DateTime? LastIncidentDate { get; set; }

        public bool IsActive { get; set; } = true;

  public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public string? AdditionalInfo { get; set; }
    }

    /// <summary>
    /// DTO for creating a new risk zone
    /// </summary>
    public class CreateRiskZoneDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

     [Required]
      public double Latitude { get; set; }

        [Required]
  public double Longitude { get; set; }

        public double Radius { get; set; } = 1.0;

     [Required]
  public string RiskLevel { get; set; } = "Medium";

   public string RiskType { get; set; } = "Crime";

        public string Description { get; set; } = string.Empty;
}

    /// <summary>
    /// DTO for updating risk zone information
    /// </summary>
    public class UpdateRiskZoneDto
    {
        public string? RiskLevel { get; set; }

   public string? Description { get; set; }

        public int? IncidentCount { get; set; }

  public bool? IsActive { get; set; }

        public string? AdditionalInfo { get; set; }
 }

    /// <summary>
    /// DTO for checking if a location is within a risk zone
    /// </summary>
    public class CheckLocationDto
    {
        [Required]
     public double Latitude { get; set; }

        [Required]
   public double Longitude { get; set; }
  }
}
