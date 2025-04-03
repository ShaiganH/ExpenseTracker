using System;
using System.Security.Claims;

namespace dotnet_project2.Extensions;

public static class ClaimsExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
{
    return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}

}
