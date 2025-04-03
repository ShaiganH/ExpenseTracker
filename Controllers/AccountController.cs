using System;
using dotnet_project2.Data;
using dotnet_project2.DTOs.User;
using dotnet_project2.Interfaces;
using dotnet_project2.Models;
using dotnet_project2.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_project2.Controllers;

[Route("api/user")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    private readonly ITokenService tokenService;
    public AccountController(ITokenService tokenService,UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> _signInManager,ApplicationDbContext _context)
    {
        this._signInManager = _signInManager;
        this._userManager = _userManager;
        this._context = _context;
        this.tokenService=tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO user)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ExistingUser = await _userManager.FindByEmailAsync(user.Email);
            if (ExistingUser != null)
            {
                return BadRequest("User already exists");
            }
            var NewUser = new ApplicationUser
            {
                UserName = user.Username,
                Email = user.Email
            };
            var Result = await _userManager.CreateAsync(NewUser, user.Password);
            if (Result.Succeeded)
            {
                return Created("User created successfully",new NewUserDto{
                    Username = NewUser.UserName,
                    Email = NewUser.Email,
                    Token = tokenService.CreateToken(NewUser)
                });
            }
            return BadRequest(Result.Errors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var User = await _userManager.FindByEmailAsync(login.Email);
            if(User == null)
            {
                return Unauthorized();
            }
            var Result = await _signInManager.CheckPasswordSignInAsync(User,login.Password,false);
            if (Result.Succeeded)
            {
                return Ok(new NewUserDto{
                    Username = User.UserName!,
                    Email = User.Email!,
                    Token = tokenService.CreateToken(User)
                });
            }
            return Unauthorized();
        }
        catch (Exception e)
        {
            return StatusCode(500,e);
        }
    }
}
