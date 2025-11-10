using System.ComponentModel.DataAnnotations;

namespace SafeRouteSharedLib.Models;

public class Companies
{
    [Key]
    public int CompanyId { get; set; }
    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;
    [Required, MaxLength(50)]
    public string CompanyRegistration { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    [Phone, MaxLength(20)]
    public string ContactNumber { get; set; } = string.Empty;

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
    public virtual ICollection<MonthlyReports> MonthlyReports { get; set; } = new List<MonthlyReports>();
}
