namespace SafeRoute.DTOs.Routes;

public class RouteDetailDto : RouteListDto
{
    public int PanicAlertsCount { get; set; }
    public int IncidentsCount { get; set; }
}