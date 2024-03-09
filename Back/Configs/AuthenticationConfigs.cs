using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.Configs;

public static class AuthenticationConfigs
{
    public const string BearerScheme = "Bearer";
    public const string CookieScheme = "Cookie";

    public static AuthenticationBuilder AddOAuthGoogle(this AuthenticationBuilder builder, AuthSettings settings)
    {

        return builder;
    }

    public static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, AuthSettings settings)
    {

        return builder;
    }

    public static void AddAuthenticationConfigs(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetService<AuthSettings>()!;

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(settings.SecurityKey)
            ),

            ValidAlgorithms = [ "HS256" ],

            ValidateAudience = true,
            ValidAudience = settings.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            RoleClaimType = "role",
        };

        services.AddAuthentication(CookieScheme)
            .AddCookie(CookieScheme)
            .AddOAuth("Google", options =>
            {
                options.Scope.Add("openid");
                options.Scope.Add("email");
                options.SaveTokens = true;
                options.SignInScheme = CookieScheme;
                options.CallbackPath = "/signin-google";
                options.ClientId = settings.GoogleClientId;
                options.ClientSecret = settings.GoogleClientSecret;
                options.TokenEndpoint = "https://oauth2.googleapis.com/token";
                options.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
                options.UserInformationEndpoint = "https://openidconnect.googleapis.com/v1/userinfo";

                options.ClaimActions.MapJsonKey("email", "email");

                options.Events.OnCreatingTicket = async authCtx =>
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, authCtx.Options.UserInformationEndpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authCtx.AccessToken);
                    using var response = await authCtx.Backchannel.SendAsync(request);
                    var user = await response.Content.ReadFromJsonAsync<JsonElement>();
                    authCtx.RunClaimActions(user);
                };
            })
            .AddJwtBearer(BearerScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
    }
}
