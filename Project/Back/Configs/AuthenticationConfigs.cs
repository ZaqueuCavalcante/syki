using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Exato.Back.Features.Cross.Login;
using Exato.Back.Features.Cross.GenerateJWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Exato.Back.Configs;

public static class AuthenticationConfigs
{
    public const string BearerScheme = "Bearer";
    public const string BearerCookie = "X-Exato-Cookie";
    public const string AzureOIDCScheme = "AzureOIDC";

    public static void AddAuthenticationConfigs(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.Auth;

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

            ValidAlgorithms = ["HS256"],

            ValidateAudience = true,
            ValidAudience = settings.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            RoleClaimType = Claims.UserRole,
        };

        builder.Services
            .AddAuthentication(options => options.DefaultChallengeScheme = BearerScheme)
            .AddJwtBearer(BearerScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies[BearerCookie];
                        if (token.HasValue())
                        {
                            context.Token = token;
                            return Task.CompletedTask;
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddOpenIdConnect(AzureOIDCScheme, options =>
            {
                options.ClientId = settings.AzureClientId;
                options.Authority = settings.AzureAuthority;
                options.CallbackPath = "/oidc/azure-callback";
                options.ClientSecret = settings.AzureClientSecret;

                options.UsePkce = true;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ResponseType = OpenIdConnectResponseType.Code;

                options.Events.OnRedirectToIdentityProvider = ctx =>
                {
                    var host = ctx.Request.Headers["X-Forwarded-Host"].FirstOrDefault() ?? ctx.Request.Host.Value;
                    ctx.ProtocolMessage.RedirectUri = $"https://{host}/api{options.CallbackPath}";
                    return Task.CompletedTask;
                };

                options.Events.OnTokenValidated = async ctx =>
                {
                    var dbCtx = ctx.HttpContext.RequestServices.GetRequiredService<BackDbContext>();
                    var service = ctx.HttpContext.RequestServices.GetRequiredService<GenerateJWTService>();

                    var email = ctx.Principal.Claims.First(x => x.Type == "email").Value.ToLower();

                    if (!email.IsValidEmail() || !email.EndsWith("@exato.digital")) return;

                    var userExists = await dbCtx.Users.AnyAsync(x => x.Email == email);
                    if (!userExists) return;

                    var jwt = await service.Generate(email);

                    ctx.Response.AppendJWTCookie(jwt.JWT, settings);
                };
            });
    }
}
