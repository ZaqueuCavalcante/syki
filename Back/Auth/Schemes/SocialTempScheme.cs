using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.Auth.Schemes;

public static class SocialTempScheme
{
    public const string Name = "SocialTemp";
    public const string Cookie = "X-Estud-SocialTempCookie";

    public static AuthenticationBuilder AddSocialTempCookieScheme(this AuthenticationBuilder builder)
    {
        return builder.AddCookie(Name, options =>
        {
            options.Cookie.Name = Cookie;
            options.Cookie.SameSite = SameSiteMode.None;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
    }
}
