using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeRouteSharedLib.Models;

public class SafetyScore
{
    [Key] public int ScoreId { get; set; }
    public int DriverId { get; set; }
    [ForeignKey(nameof(DriverId))] public virtual Drivers? Driver { get; set; }
    public DateTime CalculatedDate { get; set; }
    public int OverallScore { get; set; }
    public int IncidentScore { get; set; }
    public double RouteCompletionScore { get; set; }
    public int TotalIncidents { get; set; }
    public int TotalRoutes { get; set; }
    public int CompletedRoutes { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
