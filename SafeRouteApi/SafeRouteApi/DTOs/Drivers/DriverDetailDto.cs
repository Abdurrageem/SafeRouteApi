namespace SafeRoute.DTOs.Drivers;

public class DriverDetailDto : DriverListDto
{
    public int RoutesCount { get; set; }
    public int IncidentsCount { get; set; }
    public int NotificationsUnread { get; set; }
}
