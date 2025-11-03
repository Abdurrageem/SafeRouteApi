using System.ComponentModel.DataAnnotations;

namespace SafeRouteApi.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        public string FullName => $"{Name} {Surname}";

        [Required]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required]
        public string VehicleRegistration { get; set; } = string.Empty;

        public string VehicleModel { get; set; } = string.Empty;
    }
}