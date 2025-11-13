namespace SafeRoute.DTOs.Routes;

public class RouteListDto
{
    public int RouteId { get; set; }
    public int DriverId { get; set; }
    public string Start { get; set; } = string.Empty;
    public string End { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string RouteStatus { get; set; } = string.Empty;
    public float EstimatedDistance { get; set; }
    public float ActualDistance { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
}