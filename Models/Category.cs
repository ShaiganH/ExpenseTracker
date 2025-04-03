using System;
using System.Text.Json.Serialization;

namespace dotnet_project2.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;

    [JsonIgnore]
    public List<Expense> Expenses { get; set; } = new List<Expense>();
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}
