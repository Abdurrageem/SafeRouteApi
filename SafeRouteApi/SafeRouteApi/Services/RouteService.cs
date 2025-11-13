using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SafeRoute.Data;
using SafeRoute.DTOs.Common;
using SafeRoute.DTOs.Routes;
using SafeRoute.Services.Interfaces;

namespace SafeRoute.Services;

public class RouteService : IRouteService
{
    private readonly SafeRouteDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<RouteService> _logger;

    public RouteService(SafeRouteDbContext db, IMapper mapper, ILogger<RouteService> logger)
    { _db = db; _mapper = mapper; _logger = logger; }

    public async Task<(IEnumerable<RouteListDto> Items, int Total)> GetRoutesAsync(PaginationParams page)
    {
        var query = _db.Routes.AsNoTracking().OrderBy(r => r.RouteId);
        var total = await query.CountAsync();
        var items = await query.Skip((page.Page - 1) * page.PageSize).Take(page.PageSize)
            .ProjectTo<RouteListDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return (items, total);
    }

    public async Task<RouteDetailDto?> GetRouteAsync(int id)
    {
        return await _db.Routes.AsNoTracking().Where(r => r.RouteId == id)
            .ProjectTo<RouteDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<RouteListDto> Items, int Total)> GetRoutesByDriverAsync(int driverId, PaginationParams page)
    {
        var query = _db.Routes.AsNoTracking().Where(r => r.DriverId == driverId).OrderBy(r => r.RouteId);
        var total = await query.CountAsync();
        var items = await query.Skip((page.Page - 1) * page.PageSize).Take(page.PageSize)
            .ProjectTo<RouteListDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return (items, total);
    }

    public async Task<int> CreateRouteAsync(CreateRouteDto dto)
    {
        var entity = new Models.Routes
        {
            DriverId = dto.DriverId,
            Start = dto.Start,
            End = dto.End,
            StartTime = dto.StartTime ?? DateTime.UtcNow,
            RouteStatus = "Planned",
            EstimatedDistance = 0,
            ActualDistance = 0,
            RiskLevel = "Low"
        };
        _db.Routes.Add(entity);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Route created {RouteId}", entity.RouteId);
        return entity.RouteId;
    }

    public async Task<bool> updateStatusAsync(int id, UpdateRouteStatusDto dto)
    {
        var entity = await _db.Routes.FindAsync(id);
        if (entity == null) return false;
        entity.RouteStatus = dto.RouteStatus;
        if (dto.EndTime.HasValue) entity.EndTime = dto.EndTime.Value;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Route status updated {RouteId} -> {Status}", id, entity.RouteStatus);
        return true;
    }

    public async Task<bool> DeleteRouteAsync(int id)
    {
        var entity = await _db.Routes.FindAsync(id);
        if (entity == null) return false;
        _db.Routes.Remove(entity);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Route deleted {RouteId}", id);
        return true;
    }
}
