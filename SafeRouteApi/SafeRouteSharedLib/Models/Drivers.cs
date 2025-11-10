using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteSharedLib.Models;

public class Drivers
{
    [Key]
    public int DriverId { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual Users? User { get; set; }
    public int? DispatcherId { get; set; }
    [ForeignKey(nameof(DispatcherId))] public virtual Dispatchers? Dispatcher { get; set; }

    [Required, MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Surname { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string LicenseNumber { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string VehicleRegistration { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string VehicleModel { get; set; } = string.Empty;
    [Required] public int PackageAmount { get; set; }

    public virtual ICollection<EmergencyContacts> EmergencyContacts { get; set; } = new List<EmergencyContacts>();
    public virtual ICollection<Routes> Routes { get; set; } = new List<Routes>();
    public virtual ICollection<Incidents> Incidents { get; set; } = new List<Incidents>();
    public virtual ICollection<PanicAlerts> PanicAlerts { get; set; } = new List<PanicAlerts>();
    public virtual ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
    public virtual ICollection<ThreatDetections> ThreatDetections { get; set; } = new List<ThreatDetections>();
    public virtual ICollection<CameraRecordings> CameraRecordings { get; set; } = new List<CameraRecordings>();
    public virtual ICollection<SafetyScore> SafetyScores { get; set; } = new List<SafetyScore>();
    public virtual ICollection<IncidentResponses> IncidentResponses { get; set; } = new List<IncidentResponses>();
}
