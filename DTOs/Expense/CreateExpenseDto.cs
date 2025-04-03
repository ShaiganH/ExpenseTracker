using System;

namespace dotnet_project2.DTOs.Expense;

public class CreateExpenseDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
}
