namespace Syki.Back.Extensions;

public static class ResultExtensions
{
    public static void ThrowOnError<S, E>(this OneOf<S, E> value)
    {
        if (value.IsError()) throw new Exception((value.GetError() as SykiError).Message);
    }

    public static void AppendSykiJwtCookie(this HttpResponse response, string token, AuthSettings settings)
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
