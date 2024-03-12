using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.LoginWithGoogle;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginWithGoogleController(LoginWithGoogleService service) : ControllerBase
{
    [HttpGet("login/google")]
    public IActionResult LoginWithGoogle()
    {
        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "/login/google/callback"
            },
            [ "Google" ]
        );  
    }

    [HttpGet("login/google/callback")]
    [Authorize(AuthenticationSchemes = "Cookie")]
    public async Task<IActionResult> LoginWithGoogleCallback([FromServices] EmailSettings settings)
    {
        var token = await service.Login(User.Email());

        return Redirect($"{settings.FrontUrl}/login-oauth?token={token.AccessToken}");
    }
}
