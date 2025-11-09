using AutoMapper;
using SafeRouteApi.Models;
using SafeRouteApi.DTOs.Drivers;
using SafeRouteApi.DTOs.Routes;

namespace SafeRouteApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Drivers, DriverListDto>();
        CreateMap<Drivers, DriverDetailDto>()
            .ForMember(d => d.RoutesCount, o => o.MapFrom(s => s.Routes.Count))
            .ForMember(d => d.IncidentsCount, o => o.MapFrom(s => s.Incidents.Count))
            .ForMember(d => d.NotificationsUnread, o => o.MapFrom(s => s.Notifications.Count(n => !n.IsRead)));

        CreateMap<Routes, RouteListDto>();
        CreateMap<Routes, RouteDetailDto>()
            .ForMember(d => d.PanicAlertsCount, o => o.MapFrom(s => s.PanicAlerts.Count))
            .ForMember(d => d.IncidentsCount, o => o.MapFrom(s => s.Incidents.Count));
    }
}
