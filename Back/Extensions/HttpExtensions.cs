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
    }
}
