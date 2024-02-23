using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Syki.Back.CreateUser;
using Syki.Shared.CreateUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[EnableRateLimiting("Small")]
[ApiController, Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly SignInManager<SykiUser> _signInManager;

    public UsersController(
        IAuthService authService,
        SignInManager<SykiUser> signInManager
    ) {
        _authService = authService;
        _signInManager = signInManager;
    }

    [HttpPost()]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> Register([FromBody] CreateUserIn data)
    {
        var user = await _authService.Register(data);

        return Ok(user);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordIn data)
    {
        var ok = await _authService.ResetPassword(data);

        return Ok(ok);
    }

    [HttpGet()]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _authService.GetAllUsers();

        return Ok(users);
    }
}
