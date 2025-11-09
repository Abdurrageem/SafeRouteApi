using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SafeRouteApi.Data;
using SafeRouteApi.DTOs.Common;
using SafeRouteApi.DTOs.Drivers;
using SafeRouteApi.Services.Interfaces;

namespace SafeRouteApi.Services;

public class DriverService : IDriverService
{
    private readonly SafeRouteDbContext _db;
    private readonly IMapper _mapper;
    private readonly ILogger<DriverService> _logger;

    public DriverService(SafeRouteDbContext db, IMapper mapper, ILogger<DriverService> logger)
    {
        _db = db; _mapper = mapper; _logger = logger;
    }

    public async Task<(IEnumerable<DriverListDto> Items, int Total)> GetDriversAsync(PaginationParams page)
    {
        var query = _db.Drivers.AsNoTracking().OrderBy(d => d.DriverId);
        var total = await query.CountAsync();
        var items = await query.Skip((page.Page - 1) * page.PageSize).Take(page.PageSize)
            .ProjectTo<DriverListDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return (items, total);
    }

    public async Task<DriverDetailDto?> GetDriverAsync(int id)
    {
        return await _db.Drivers.AsNoTracking()
            .Where(d => d.DriverId == id)
            .ProjectTo<DriverDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateDriverAsync(string name, string surname, string licenseNumber, string vehicleReg, string vehicleModel, int userId)
    {
        var entity = new Models.Drivers
        {
            UserId = userId,
            Name = name,
            Surname = surname,
            LicenseNumber = licenseNumber,
            VehicleRegistration = vehicleReg,
            VehicleModel = vehicleModel
        };
        _db.Drivers.Add(entity);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Driver created: {DriverId}", entity.DriverId);
        return entity.DriverId;
    }

    public async Task<bool> UpdateDriverAsync(int id, string? name, string? surname, string? vehicleReg, string? vehicleModel)
    {
        var entity = await _db.Drivers.FindAsync(id);
        if (entity == null) return false;
        if (name != null) entity.Name = name;
        if (surname != null) entity.Surname = surname;
        if (vehicleReg != null) entity.VehicleRegistration = vehicleReg;
        if (vehicleModel != null) entity.VehicleModel = vehicleModel;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Driver updated: {DriverId}", id);
        return true;
    }

    public async Task<bool> DeleteDriverAsync(int id)
    {
        var entity = await _db.Drivers.FindAsync(id);
        if (entity == null) return false;
        _db.Drivers.Remove(entity);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Driver deleted: {DriverId}", id);
        return true;
    }
}
