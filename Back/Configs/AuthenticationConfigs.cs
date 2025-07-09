using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Syki.Back.Configs;

public static class AuthenticationConfigs
{
    public const string BearerScheme = "Bearer";

    public static void AddAuthenticationConfigs(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.Auth();

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

        builder.Services.AddAuthentication(BearerScheme)
            .AddJwtBearer(BearerScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var path = context.HttpContext.Request.Path;

                        var webSocketJwt = context.Request.Query["access_token"];
                        if (webSocketJwt.HasValue() && path.StartsWithSegments("/syki-hub"))
                        {
                            context.Token = webSocketJwt;
                            return Task.CompletedTask;
                        }

                        var cookieJwt = context.Request.Cookies["syki_jwt"];
                        if (cookieJwt.HasValue())
                        {
                            context.Token = cookieJwt;
                            return Task.CompletedTask;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }
}
