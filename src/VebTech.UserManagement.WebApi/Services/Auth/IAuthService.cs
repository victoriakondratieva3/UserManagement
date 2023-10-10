namespace VebTech.UserManagement.WebApi.Services.Auth;

using WebApi.Models;

public interface IAuthService
{
    public string GenerateJwtToken(LoginRequest model);
}