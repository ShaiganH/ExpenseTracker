using System;
using dotnet_project2.Data;
using dotnet_project2.DTOs.Expense;
using dotnet_project2.Extensions;
using dotnet_project2.Helpers;
using dotnet_project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public async Task<IActionResult> GetExpenses([FromQuery] ExpenseQueryObject expenseQuery)
    {
        var UserId = User.GetUserId();
        if (string.IsNullOrEmpty(UserId))
        {
            return Unauthorized("User ID not found in token.");
        }
        var expenses =  _context.Expenses
            .Where(x=> x.ApplicationUserId == UserId)
            .Include(x => x.Category)
            .AsQueryable();
        
        var Page = (expenseQuery.PageNumber - 1)*expenseQuery.PageSize;
        var paginatedExpenses = await expenses.Skip(Page).Take(expenseQuery.PageSize).ToListAsync();
        return Ok(paginatedExpenses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto createExpenseDto,[FromServices] IHubContext<ExpenseHub> hubContext)
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
        await hubContext.Clients.All.SendAsync("ExpenseAdded", NewExpense);
        return Ok(NewExpense);
    }


    [HttpGet("insights")]
    public async Task<IActionResult> GetFinancialInsights()
    {
        var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var expenses = await _context.Expenses
            .Where(x => x.ApplicationUserId == userId)
            .Include(x => x.Category)
            .ToListAsync();

        if (!expenses.Any()) return Ok("No spending data available");

        var totalSpent = expenses.Sum(e => e.Amount);
        var categoryWiseSpending = expenses
            .GroupBy(e => e.Category.Name)
            .Select(group => new { Category = group.Key, Total = group.Sum(e => e.Amount) })
            .OrderByDescending(e => e.Total)
            .ToList();

        return Ok(new { TotalSpent = totalSpent, CategoryBreakdown = categoryWiseSpending });    
    }
}
