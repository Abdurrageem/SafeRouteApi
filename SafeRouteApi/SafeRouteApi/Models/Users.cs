using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRoute.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        // Foreign key for company
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Companies? Company { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        // Navigation properties
        public virtual Drivers? Driver { get; set; }
        public virtual Dispatchers? Dispatcher { get; set; }
    }
}
