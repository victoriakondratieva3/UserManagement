namespace VebTech.UserManagement.WebApi.Controllers;

using Models;
using Helpers;
using Services.Auth;
using Services.User;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration Configuration;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration configuration, IUserService userService, IAuthService authService)
    {
        Configuration = configuration;
        _userService = userService;
        _authService = authService;
    }

    /// <summary>
    /// Authenticates the User
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Auth([FromQuery] LoginRequest model)
    {
        Logger.Info("Requested GET: api/Auth");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var isDataValid = _userService.IsValidUserData(model);
        if (isDataValid)
        {
            var tokenString = _authService.GenerateJwtToken(model);
            return Ok(new { Token = tokenString, Message = "Success" });
        }

        return BadRequest("Invalid authentication data");
    }

    /// <summary>
    /// Сhecks User authenticate
    /// </summary>
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet(nameof(GetResult))]
    public IActionResult GetResult()
    {
        Logger.Info("Requested GET: api/GetResult");

        return Ok("Authorized");
    }
}
