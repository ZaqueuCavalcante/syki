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
    public async Task<IActionResult> LoginWithGoogleCallback()
    {
        var token = await service.Login(User.Email());

        // Get from settings

        return Redirect($"https://localhost:6001/login-oauth?token={token.AccessToken}");
    }
}
