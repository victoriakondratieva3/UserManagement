namespace VebTech.UserManagement.WebApi.Services.Auth;

using Models;

public interface IAuthService
{
    public string GenerateJwtToken(LoginRequest model);
}