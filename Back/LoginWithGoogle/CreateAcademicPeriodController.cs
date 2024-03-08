using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Syki.Back.LoginWithGoogle;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginWithGoogleController() : ControllerBase
{
    [HttpGet("login-with-google")]
    public IActionResult LoginWithGoogle()
    {
        return Challenge(
            new AuthenticationProperties
            {  
                RedirectUri = "https://accounts.google.com/o/oauth2/v2/auth"  
            },
            OpenIdConnectDefaults.AuthenticationScheme
        );  
    }
}
