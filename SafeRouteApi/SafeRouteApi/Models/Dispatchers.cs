using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRoute.Models
{
    public class Dispatchers
    {
        [Key]
        public int DispatcherId { get; set; }

        // Foreign key for users
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual Users? User { get; set; }

        public DateTime ShiftStartTime { get; set; }

        public DateTime? ShiftEndTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string ShiftPattern { get; set; } = string.Empty;

        public bool IsOnDuty { get; set; }

        // Navigation property
        public virtual ICollection<Drivers> Drivers { get; set; } = new List<Drivers>();
    }
}
