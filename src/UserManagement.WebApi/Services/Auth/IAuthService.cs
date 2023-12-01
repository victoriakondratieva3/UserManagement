using UserManagement.WebApi.Models;

namespace UserManagement.WebApi.Services.Auth;

public interface IAuthService
{
    public string GenerateJwtToken(LoginRequest model);
}