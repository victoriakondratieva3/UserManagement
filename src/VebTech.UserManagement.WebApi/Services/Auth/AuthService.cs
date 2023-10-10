namespace VebTech.UserManagement.WebApi.Services.Auth;

using WebApi.Models;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly IConfiguration Configuration;

    public AuthService(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public string GenerateJwtToken(LoginRequest model)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration["JWT:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", model.Email) }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = Configuration["JWT:Issuer"],
            Audience = Configuration["JWT:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
