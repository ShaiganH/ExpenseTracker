using System;
using dotnet_project2.Models;
using Microsoft.AspNetCore.Identity;
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
    public DbSet<Achievement> Achievements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Expense>()
            .HasIndex(e => e.ApplicationUserId);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole {Name = "Admin",NormalizedName = "ADMIN"},
            new IdentityRole {Name = "User",NormalizedName = "USER"}
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }


}
