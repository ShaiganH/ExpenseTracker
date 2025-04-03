using System;
using dotnet_project2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project2.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Budget> Budgets { get; set; }

}
