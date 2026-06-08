using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HKtab.Backend.DTOs;
using HKtab.Backend.Services;

namespace HKtab.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var userId = GetUserId();
        var expenses = await _expenseService.GetExpensesAsync(userId);
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpense(int id)
    {
        var userId = GetUserId();
        var expense = await _expenseService.GetExpenseAsync(userId, id);
        
        if (expense == null)
            return NotFound();

        return Ok(expense);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var expense = await _expenseService.CreateExpenseAsync(userId, request);
        
        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var success = await _expenseService.UpdateExpenseAsync(userId, id, request);
        
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var userId = GetUserId();
        var success = await _expenseService.DeleteExpenseAsync(userId, id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}
