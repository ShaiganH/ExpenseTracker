using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotnet_project2.Interfaces;
using dotnet_project2.Models;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_project2.Service;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    private readonly SymmetricSecurityKey key;
    public TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));
    }
    public string CreateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"]
        };

        var TokenHandler = new JwtSecurityTokenHandler();
        var token = TokenHandler.CreateToken(tokenDescriptor);
        return TokenHandler.WriteToken(token);
    }
}
