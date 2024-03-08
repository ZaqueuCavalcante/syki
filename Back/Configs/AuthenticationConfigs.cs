using System.Text;
using Syki.Back.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Syki.Back.Configs;

public static class AuthenticationConfigs
{
    public const string BearerScheme = "Bearer";

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

        // services.AddAccessTokenManagement();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = BearerScheme;
            options.DefaultSignInScheme = BearerScheme;
            options.DefaultChallengeScheme = BearerScheme;
            options.DefaultAuthenticateScheme = BearerScheme;
        })
        .AddJwtBearer(BearerScheme, options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            // How middleware persists the user identity? (Cookie)
            options.SignInScheme = BearerScheme;

            // How Broswer redirects user to authentication provider?
            options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

            // How response should be sent back from authentication provider?
            options.ResponseMode = OpenIdConnectResponseMode.FormPost;

            options.Authority = "https://accounts.google.com";
            options.ClientId = "lalala";
            options.ClientSecret = "lalala";
            options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            options.UsePkce = true;

            // Where we would like to get the response after authentication?
            options.CallbackPath = "/signin-google";

            // Where we would like to get the response after log out?
            options.SignedOutCallbackPath = "/signout-callback-oidc";

            // Should we persist tokens?
            options.SaveTokens = true;

            // Should we request user profile details for user end point?
            options.GetClaimsFromUserInfoEndpoint = true;

            // What scopes do we need?
            options.Scope.Add("openid");
            options.Scope.Add("email");
            options.Scope.Add("phone");
            options.Scope.Add("profile");

            // What claims from above scopes do we need?
            // unblock the required claims by using Remove()
            options.ClaimActions.Remove("openid");
            options.ClaimActions.Remove("email");
            options.ClaimActions.Remove("phone");
            options.ClaimActions.Remove("profile");

            // How to handle OIDC events?
            options.Events = new OpenIdConnectEvents
            {
                // Where to redirect after we get logout redirection?
                OnSignedOutCallbackRedirect = context =>
                {
                    context.Response.Redirect("/");
                    context.HandleResponse();
                    return Task.FromResult(0);
                },

                // Where to redirect when we get authentication errors?
                OnRemoteFailure = context =>
                {
                    context.Response.Redirect("/");
                    context.HandleResponse();
                    return Task.FromResult(0);
                }
            };
        });
    }
}
