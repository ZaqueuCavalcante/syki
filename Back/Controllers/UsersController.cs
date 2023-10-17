using Syki.Dtos;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SykiUser = Syki.Back.Domain.SykiUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly SignInManager<SykiUser> _signInManager;
    private readonly UserManager<SykiUser> _userManager;

    public UsersController(
        AuthService authService,
        SignInManager<SykiUser> signInManager,
        UserManager<SykiUser> userManager
    ) {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("register")]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> Register([FromBody] RegisterIn body)
    {
        await _authService.Register(body);

        return Ok();
    }

    [HttpGet("mfa-key")]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> GetMfaKey()
    {
        var key = await _authService.GetMfaKey(User.Id());

        return Ok(new { key });
    }

    [HttpPost("mfa-setup")]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> MfaSetup([FromBody] MfaSetupIn body)
    {
        await _authService.SetupMfa(User.Id(), body.Token);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginIn body)
    {
        var result = await _signInManager.PasswordSignInAsync(
            userName: body.Email,
            password: body.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (result.RequiresTwoFactor)
        {
            var provider = _userManager.Options.Tokens.AuthenticatorTokenProvider;
            result = await _signInManager.TwoFactorSignInAsync(provider, body.TwoFactorToken!, false, false);
        }

        if (!result.Succeeded)
        {
            return Unauthorized("Login failed.");
        }

        var jwt = await _authService.GenerateAccessToken(body.Email);

        return Ok(new LoginOut { AccessToken = jwt });
    }
}
