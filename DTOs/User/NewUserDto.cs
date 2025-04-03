using System;

namespace dotnet_project2.DTOs.User;

public class NewUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
