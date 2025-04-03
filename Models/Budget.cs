using System;

namespace dotnet_project2.Models;

public class Budget
{
    public int BudgetId { get; set; }
    public decimal TotalBudget { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }


}
