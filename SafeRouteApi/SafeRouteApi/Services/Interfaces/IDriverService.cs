using SafeRoute.DTOs.Common;
using SafeRoute.DTOs.Drivers;

namespace SafeRoute.Services.Interfaces;

public interface IDriverService
{
    Task<(IEnumerable<DriverListDto> Items, int Total)> GetDriversAsync(PaginationParams page);
    Task<DriverDetailDto?> GetDriverAsync(int id);
    Task<int> CreateDriverAsync(string name, string surname, string licenseNumber, string vehicleReg, string vehicleModel, int userId);
    Task<bool> UpdateDriverAsync(int id, string? name, string? surname, string? vehicleReg, string? vehicleModel);
    Task<bool> DeleteDriverAsync(int id);
}
