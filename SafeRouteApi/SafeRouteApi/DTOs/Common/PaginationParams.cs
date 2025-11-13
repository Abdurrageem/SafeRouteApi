using System.ComponentModel.DataAnnotations;

namespace SafeRoute.DTOs.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    private int _pageSize = 25;

    [Range(1, MaxPageSize)]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
