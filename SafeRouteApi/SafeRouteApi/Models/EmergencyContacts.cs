using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteApi.Models
{
    public class EmergencyContacts
    {
        [Key]
        public int ContactId { get; set; }

        // Foreign key for drivers
        public int DriverId { get; set; }

        [ForeignKey(nameof(DriverId))]
        public virtual Drivers? Driver { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Relationship { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
    }
}
