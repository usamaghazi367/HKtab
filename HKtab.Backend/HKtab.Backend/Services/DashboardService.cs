using HKtab.Backend.Data;
using HKtab.Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HKtab.Backend.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(int userId)
    {
        var incomes = await _context.Incomes
            .Where(i => i.UserId == userId)
            .ToListAsync();

        var expenses = await _context.Expenses
            .Where(e => e.UserId == userId)
            .ToListAsync();

        var totalIncome = incomes.Sum(i => i.Amount);
        var totalExpense = expenses.Sum(e => e.Amount);

        var expenseCategories = expenses
            .GroupBy(e => e.Category)
            .Select(g => new CategorySummaryDto
            {
                Category = g.Key,
                Amount = g.Sum(e => e.Amount)
            })
            .OrderByDescending(c => c.Amount)
            .Take(5)
            .ToList();

        var recentTransactions = incomes
            .Select(i => new TransactionDto
            {
                Id = i.Id,
                Type = "Income",
                Amount = i.Amount,
                Description = string.IsNullOrWhiteSpace(i.Description) ? i.Category : i.Description,
                Date = i.Date
            })
            .Concat(expenses.Select(e => new TransactionDto
            {
                Id = e.Id,
                Type = "Expense",
                Amount = e.Amount,
                Description = string.IsNullOrWhiteSpace(e.Description) ? e.Category : e.Description,
                Date = e.Date
            }))
            .OrderByDescending(t => t.Date)
            .Take(10)
            .ToList();

        return new DashboardSummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            Balance = totalIncome - totalExpense,
            TransactionCount = incomes.Count + expenses.Count,
            TopCategories = expenseCategories,
            RecentTransactions = recentTransactions
        };
    }
}
