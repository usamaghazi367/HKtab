using HKtab.Backend.Data;
using HKtab.Backend.DTOs;
using HKtab.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HKtab.Backend.Services;

public class ExpenseService : IExpenseService
{
    private readonly ApplicationDbContext _context;

    public ExpenseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync(int userId)
    {
        return await _context.Expenses
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date)
            .Select(e => MapToDto(e))
            .ToListAsync();
    }

    public async Task<ExpenseDto?> GetExpenseAsync(int userId, int id)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

        return expense == null ? null : MapToDto(expense);
    }

    public async Task<ExpenseDto> CreateExpenseAsync(int userId, CreateExpenseRequest request)
    {
        var expense = new Expense
        {
            UserId = userId,
            Amount = request.Amount,
            Category = request.Category,
            Description = request.Description,
            Date = request.Date
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        return MapToDto(expense);
    }

    public async Task<bool> UpdateExpenseAsync(int userId, int id, UpdateExpenseRequest request)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

        if (expense == null)
            return false;

        expense.Amount = request.Amount;
        expense.Category = request.Category;
        expense.Description = request.Description;
        expense.Date = request.Date;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteExpenseAsync(int userId, int id)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

        if (expense == null)
            return false;

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ExpenseDto MapToDto(Expense expense) => new()
    {
        Id = expense.Id,
        Amount = expense.Amount,
        Category = expense.Category,
        Description = expense.Description,
        Date = expense.Date,
        CreatedAt = expense.CreatedAt
    };
}
