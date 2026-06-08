using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HKtab.Backend.DTOs;
using HKtab.Backend.Services;

namespace HKtab.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IncomesController : ControllerBase
{
    private readonly IIncomeService _incomeService;

    public IncomesController(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    }

    [HttpGet]
    public async Task<IActionResult> GetIncomes()
    {
        var userId = GetUserId();
        var incomes = await _incomeService.GetIncomesAsync(userId);
        return Ok(incomes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncome(int id)
    {
        var userId = GetUserId();
        var income = await _incomeService.GetIncomeAsync(userId, id);
        
        if (income == null)
            return NotFound();

        return Ok(income);
    }

    [HttpPost]
    public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var income = await _incomeService.CreateIncomeAsync(userId, request);
        
        return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(int id, [FromBody] UpdateIncomeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var success = await _incomeService.UpdateIncomeAsync(userId, id, request);
        
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(int id)
    {
        var userId = GetUserId();
        var success = await _incomeService.DeleteIncomeAsync(userId, id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}
