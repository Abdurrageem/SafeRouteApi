using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Models
{
    /// <summary>
    /// Notification model matching your MAUI app's structure
    /// </summary>
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public int DriverId { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty; // Safety, Weather, Traffic, System

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public string Priority { get; set; } = "Medium"; // High, Medium, Low

        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.Now;

        public DateTime? ReadAt { get; set; }
    }
}