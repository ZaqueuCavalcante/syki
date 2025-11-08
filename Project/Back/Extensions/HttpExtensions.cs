using Microsoft.AspNetCore.Mvc.Controllers;

namespace Exato.Back.Extensions;

public static class HttpExtensions
{
    extension(HttpResponse response)
    {
        public void AppendJWTCookie(string token, AuthSettings settings)
        {
            response.Cookies.Append(
                AuthenticationConfigs.BearerCookie,
                token,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = settings.CookieSecure,
                }
            );
        }
    }

    extension(HttpContext context)
    {
        public string GetTargetControllerName()
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null) return "NOT_FOUND";

            var action = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (action == null) return "NOT_FOUND";

            return $"{action.ControllerName}Controller";
        }

        public void ClearOidcStateCookies()
        {
            var cookie = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Path = "/api/oidc/azure-callback",
            };

            foreach (var key in context.Request.Cookies.Keys)
            {
                if (key.StartsWith(".AspNetCore.Correlation.", StringComparison.OrdinalIgnoreCase) ||
                    key.StartsWith(".AspNetCore.OpenIdConnect.Nonce.", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.Cookies.Delete(key, cookie);
                }
            }
        }
    }
}
