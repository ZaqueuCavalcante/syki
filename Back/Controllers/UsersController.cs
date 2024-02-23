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

    [Authorize]
    [HttpGet("mfa-key")]
    public async Task<IActionResult> GetMfaKey()
    {
        var key = await _authService.GetMfaKey(User.Id());

        return Ok(new MfaKeyOut { Key = key });
    }

    [Authorize]
    [HttpPost("mfa-setup")]
    public async Task<IActionResult> SetupMfa([FromBody] MfaSetupIn data)
    {
        var ok = await _authService.SetupMfa(User.Id(), data.Token);

        return Ok(ok);
    }

    [HttpPost("login-mfa")]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn data)
    {
        // TODO: remove logic from controller
        var token = data.Code!.OnlyNumbers();
        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(token, false, false);

        if (!result.Succeeded)
        {
            return BadRequest(new LoginOut { Wrong2FactorCode = true });
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        var jwt = await _authService.GenerateAccessToken(user!.Email!);

        return Ok(new LoginOut { AccessToken = jwt });
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
