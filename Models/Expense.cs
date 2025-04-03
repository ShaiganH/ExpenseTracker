using System;

namespace dotnet_project2.Models;

public class Expense
{
    public int ExpenseId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime DateOfExpense { get; set; } = DateTime.Now;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

}
