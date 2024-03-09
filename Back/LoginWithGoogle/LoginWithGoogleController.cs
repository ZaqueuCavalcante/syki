using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.LoginWithGoogle;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginWithGoogleController(LoginWithGoogleService service) : ControllerBase
{
    [HttpGet("oauth-google")]
    public IActionResult OAuthGoogle()
    {
        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "/login-with-google"
            },
            [ "Google" ]
        );  
    }

    [HttpGet("login-with-google")]
    [Authorize(AuthenticationSchemes = "Cookie")]
    public async Task<IActionResult> LoginWithGoogle()
    {
        var token = await service.Login(User.Email());

        return Redirect($"https://localhost:6001/login-oauth?token={token.AccessToken}");
    }
}
