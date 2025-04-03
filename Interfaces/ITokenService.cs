using System;
using dotnet_project2.Models;

namespace dotnet_project2.Interfaces;


public interface ITokenService
{
    public string CreateToken(ApplicationUser user);
}

