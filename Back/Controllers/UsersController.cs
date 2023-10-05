using Syki.Dtos;
using Syki.Back.Services;
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

    public UsersController(
        AuthService authService,
        SignInManager<SykiUser> signInManager
    ) {
        _authService = authService;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> Register([FromBody] RegisterIn body)
    {
        await _authService.Register(body);

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

        if (!result.Succeeded)
        {
            return Unauthorized("Login failed.");
        }

        var jwt = await _authService.GenerateAccessToken(body.Email);

        return Ok(new LoginOut { AccessToken = jwt });
    }
}
