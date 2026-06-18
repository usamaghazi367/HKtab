using HKtab.Backend.DTOs;

namespace HKtab.Backend.Services;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseDto>> GetExpensesAsync(int userId);
    Task<ExpenseDto?> GetExpenseAsync(int userId, int id);
    Task<ExpenseDto> CreateExpenseAsync(int userId, CreateExpenseRequest request);
    Task<bool> UpdateExpenseAsync(int userId, int id, UpdateExpenseRequest request);
    Task<bool> DeleteExpenseAsync(int userId, int id);
}
