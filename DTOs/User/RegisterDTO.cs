using System;
using System.ComponentModel.DataAnnotations;

namespace dotnet_project2.DTOs.User;

public class RegisterDTO
{
    [Required]
    [MaxLength(20,ErrorMessage = "Username must be less than 20 characters")]
    [MinLength(3,ErrorMessage = "Username must be more than 3 characters")]
    public string Username { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(6,ErrorMessage = "Password must be more than 6 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
}
