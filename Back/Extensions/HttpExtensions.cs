using Syki.Back.Auth.Schemes;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Syki.Back.Extensions;

public static partial class HttpExtensions
{
    extension(HttpResponse response)
    {
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
                    MaxAge = TimeSpan.FromMinutes(settings.ExpirationTimeInMinutes),
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

    [GeneratedRegex(@"[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}", RegexOptions.IgnoreCase)]
    private static partial Regex GuidPattern();

    private static string NormalizePath(string path) => GuidPattern().Replace(path, "{id}");

    extension(HttpContext context)
    {
        public string GetTargetControllerName()
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null) return NormalizePath(context.Request.Path.ToString());

            var action = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (action != null) return $"{action.ControllerName}Controller";

            if (endpoint is RouteEndpoint routeEndpoint)
                return routeEndpoint.RoutePattern.RawText ?? endpoint.DisplayName ?? NormalizePath(context.Request.Path.ToString());

            return endpoint.DisplayName ?? NormalizePath(context.Request.Path.ToString());
        }

        public string GetIpAddress()
        {
            return context.Connection.RemoteIpAddress?.ToString() ?? "-";
        }

        public string GetUserAgent()
        {
            return context.Request.Headers.UserAgent.FirstOrDefault() ?? "-";
        }

        public async Task SignInTwoFactorUserIdSchemeAsync(int userId)
        {
            // Store user ID in Identity cookie for 2FA verification step
            var identity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userId.ToString()));
            await context!.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, new ClaimsPrincipal(identity));
        }
    }
}
