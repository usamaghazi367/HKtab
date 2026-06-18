using HKtab.Backend.DTOs;

namespace HKtab.Backend.Services;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync(int userId);
}
