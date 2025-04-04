using System;

namespace dotnet_project2.Models;

public class Achievement
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
