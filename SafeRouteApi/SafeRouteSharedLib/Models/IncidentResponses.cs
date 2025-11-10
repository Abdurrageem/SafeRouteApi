using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteSharedLib.Models;

public class IncidentResponses
{
    [Key] public int IncidentResponseId { get; set; }
    public int IncidentId { get; set; }
    [ForeignKey(nameof(IncidentId))] public virtual Incidents? Incident { get; set; }
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))] public virtual Drivers? Driver { get; set; }

    [Required, MaxLength(100)] public string Type { get; set; } = string.Empty;
    public DateTime Time { get; set; }
    [MaxLength(2000)] public string Details { get; set; } = string.Empty;
    public bool WasSuccessful { get; set; }
}
