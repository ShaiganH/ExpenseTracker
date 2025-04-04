using System;
using dotnet_project2.Data;
using Microsoft.EntityFrameworkCore;
using dotnet_project2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dotnet_project2.Extensions;
using dotnet_project2.DTOs.Category;
using dotnet_project2.Helpers;

namespace dotnet_project2.Controllers;

[ApiController]
[Route("api/category")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> userManager;
    public CategoryController(ApplicationDbContext _context,UserManager<ApplicationUser> userManager)
    {
        this._context = _context;
        this.userManager = userManager;
    }

    [HttpGet]
    public IActionResult GetCategories([FromQuery] CategoryQueryObject categoryQuery)
    {
        var userId = User.GetUserId();

    if (string.IsNullOrEmpty(userId))
    {
        return Unauthorized("User ID not found in token.");
    }
    
    var categories =  _context.Categories
        .Where(x => x.ApplicationUserId == userId)
        .AsQueryable();

    if(!string.IsNullOrEmpty(categoryQuery.ByName))
    {
        categories = categories.Where(x => x.Name.Contains(categoryQuery.ByName));
    }

    return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto category)
    {
        var userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == category.Name && c.ApplicationUserId == userId);

        if (existingCategory != null) 
        {
            return BadRequest("Category already exists.");
        }

        Category newCategory = new Category(){
            Name = category.Name,
            ApplicationUserId = userId
        };

        newCategory.ApplicationUserId = userId;

        _context.Categories.Add(newCategory);
        await _context.SaveChangesAsync();

        return Ok(category);
    }
}
