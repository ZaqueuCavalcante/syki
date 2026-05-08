using System.Security.Claims;
using Syki.Back.Auth.Schemes;
using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.Extensions;

public static class HttpExtensions
{
    extension(HttpResponse response)
    {
        public void AppendSykiJwtCookie(string token, AuthSettings settings)
        {
            response.Cookies.Append(
                "syki_jwt",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = settings.CookieSecure,
                    Domain = settings.CookieDomain
                }
            );
        }

        public void AppendJWTCookie(string token, AuthSettings settings)
        {
            response.Cookies.Append(
                JwtBearerScheme.Cookie,
                token,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = settings.CookieSecure,
                    SameSite = settings.CookieSameSite,
                }
            );
        }

        public void DeleteJWTCookie(AuthSettings settings)
        {
            response.Cookies.Delete(
                JwtBearerScheme.Cookie,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    SameSite = settings.CookieSameSite,
                    Secure = settings.CookieSecure,
                }
            );
        }
    }

    extension(HttpContext context)
    {
        public async Task SignInTwoFactorUserIdSchemeAsync(Guid userId)
        {
            // Store user ID in Identity cookie for 2FA verification step
            var identity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userId.ToString()));
            await context!.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, new ClaimsPrincipal(identity));
        }
    }
}
