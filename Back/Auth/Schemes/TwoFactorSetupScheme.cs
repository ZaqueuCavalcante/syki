using Microsoft.AspNetCore.Authentication;

namespace Estud.Back.Auth.Schemes;

public static class TwoFactorSetupScheme
{
    public const string Name = "TwoFactorSetup";
    public const string Cookie = "X-Estud-TwoFactorSetupCookie";

    public static AuthenticationBuilder AddTwoFactorSetupScheme(this AuthenticationBuilder builder)
    {
        return builder.AddCookie(Name, options =>
        {
            options.Cookie.Name = Cookie;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });
    }
}
