using HKtab.Backend.Data;
using HKtab.Backend.DTOs;
using HKtab.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HKtab.Backend.Services;

public class IncomeService : IIncomeService
{
    private readonly ApplicationDbContext _context;

    public IncomeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IncomeDto>> GetIncomesAsync(int userId)
    {
        return await _context.Incomes
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.Date)
            .Select(i => MapToDto(i))
            .ToListAsync();
    }

    public async Task<IncomeDto?> GetIncomeAsync(int userId, int id)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        return income == null ? null : MapToDto(income);
    }

    public async Task<IncomeDto> CreateIncomeAsync(int userId, CreateIncomeRequest request)
    {
        var income = new Income
        {
            UserId = userId,
            Amount = request.Amount,
            Category = request.Category,
            Description = request.Description,
            Date = request.Date
        };

        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();

        return MapToDto(income);
    }

    public async Task<bool> UpdateIncomeAsync(int userId, int id, UpdateIncomeRequest request)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        if (income == null)
            return false;

        income.Amount = request.Amount;
        income.Category = request.Category;
        income.Description = request.Description;
        income.Date = request.Date;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteIncomeAsync(int userId, int id)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        if (income == null)
            return false;

        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync();
        return true;
    }

    private static IncomeDto MapToDto(Income income) => new()
    {
        Id = income.Id,
        Amount = income.Amount,
        Category = income.Category,
        Description = income.Description,
        Date = income.Date,
        CreatedAt = income.CreatedAt
    };
}
