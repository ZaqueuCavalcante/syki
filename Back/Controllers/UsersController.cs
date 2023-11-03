using Syki.Shared;
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
    [Authorize(Roles = $"{Adm}, {Academico}")]
    public async Task<IActionResult> GetMfaKey()
    {
        var key = await _authService.GetMfaKey(User.Id());

        return Ok(new MfaKeyOut { Key = key });
    }

    [HttpPost("mfa-setup")]
    [Authorize(Roles = $"{Adm}, {Academico}")]
    public async Task<IActionResult> MfaSetup([FromBody] MfaSetupIn body)
    {
        var ok = await _authService.SetupMfa(User.Id(), body.Token.OnlyNumbers());

        return Ok(new MfaSetupOut { Ok = ok });
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

        if (!result.Succeeded && !result.RequiresTwoFactor)
        {
            return BadRequest(new LoginOut { WrongEmailOrPassword = true });
        }

        if (result.RequiresTwoFactor)
        {
            var cookie = Response.Headers["Set-Cookie"][0]!.Split(";")[0];
            // Response.Headers["Set-Cookie"] = cookie;
            return BadRequest(new LoginOut { RequiresTwoFactor = true, TwoFactorUserId = cookie });
        }

        var jwt = await _authService.GenerateAccessToken(body.Email);

        return Ok(new LoginOut { AccessToken = jwt });
    }

    [HttpPost("login-mfa")]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn body)
    {
        var token = body.Code!.OnlyNumbers();
        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(token, false, false);

        if (!result.Succeeded)
        {
            return BadRequest(new LoginOut { Wrong2FactorCode = true });
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        var jwt = await _authService.GenerateAccessToken(user!.Email!);

        return Ok(new LoginOut { AccessToken = jwt });
    }
}
