using HKtab.Backend.DTOs;

namespace HKtab.Backend.Services;

public interface IIncomeService
{
    Task<IEnumerable<IncomeDto>> GetIncomesAsync(int userId);
    Task<IncomeDto?> GetIncomeAsync(int userId, int id);
    Task<IncomeDto> CreateIncomeAsync(int userId, CreateIncomeRequest request);
    Task<bool> UpdateIncomeAsync(int userId, int id, UpdateIncomeRequest request);
    Task<bool> DeleteIncomeAsync(int userId, int id);
}
