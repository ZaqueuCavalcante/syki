using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.Auth.Schemes;

public static class SsoTempScheme
{
    public const string Name = "SsoTemp";
    public const string Cookie = "X-WExato-SsoTempCookie";

    public static AuthenticationBuilder AddSsoTempCookieScheme(this AuthenticationBuilder builder)
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
