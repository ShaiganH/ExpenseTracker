using System;
using dotnet_project2.Data;
using dotnet_project2.DTOs.Expense;
using dotnet_project2.Extensions;
using dotnet_project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project2.Controllers;

[Authorize]
[Route("api/expense")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ExpenseController(ApplicationDbContext _context)
    {
        this._context = _context;
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var UserId = User.GetUserId();
        if (string.IsNullOrEmpty(UserId))
        {
            return Unauthorized("User ID not found in token.");
        }
        List<Expense> expenses = await _context.Expenses
            .Where(x=> x.ApplicationUserId == UserId)
            .Include(x => x.Category)
            .ToListAsync();
        return Ok(expenses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto createExpenseDto)
    {
        var UserId = User.GetUserId();
        if (string.IsNullOrEmpty(UserId))
        {
            return Unauthorized("User ID not found in token.");
        }
        Category category =await _context.Categories.FirstOrDefaultAsync(x=> x.Name == createExpenseDto.CategoryName && x.ApplicationUserId == UserId);

        if (category == null)
        {
            return BadRequest("Category not found");
        }
        var NewExpense = new Expense
        {
            Amount = createExpenseDto.Amount,
            Description = createExpenseDto.Description,
            PaymentMethod = createExpenseDto.PaymentMethod,
            CategoryId = category.CategoryId,
            ApplicationUserId = UserId
        };



        _context.Expenses.Add(NewExpense);
        await _context.SaveChangesAsync();
        return Ok(NewExpense);
    }

}
