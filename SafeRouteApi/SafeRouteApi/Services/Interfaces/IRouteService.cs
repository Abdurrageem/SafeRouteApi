using SafeRouteApi.DTOs.Common;
using SafeRouteApi.DTOs.Routes;

namespace SafeRouteApi.Services.Interfaces;

public interface IRouteService
{
    Task<(IEnumerable<RouteListDto> Items, int Total)> GetRoutesAsync(PaginationParams page);
    Task<RouteDetailDto?> GetRouteAsync(int id);
    Task<(IEnumerable<RouteListDto> Items, int Total)> GetRoutesByDriverAsync(int driverId, PaginationParams page);
    Task<int> CreateRouteAsync(CreateRouteDto dto);
    Task<bool> updateStatusAsync(int id, UpdateRouteStatusDto dto);
    Task<bool> DeleteRouteAsync(int id);
}
