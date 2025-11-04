using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteApi.Models
{
    public class Notifications
    {
        [Key]
        public int NotificationId { get; set; }

        // Foreign key for drivers
        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public virtual Drivers? Driver { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Priority { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime? ReadAt { get; set; }
    }
}
