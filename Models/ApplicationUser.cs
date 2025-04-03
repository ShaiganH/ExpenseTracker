using System;
using Microsoft.AspNetCore.Identity;

namespace dotnet_project2.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public List<Expense> Expenses { get; set; } = new List<Expense>();
    public List<Category> Categories { get; set; } = new List<Category>();
    public Budget? Budget { get; set; }  

}
