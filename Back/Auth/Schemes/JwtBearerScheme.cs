using System.Text;
using Syki.Back.Auth.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Syki.Back.Auth.Schemes;

public static class JwtBearerScheme
{
    public const string Name = "Bearer";
    public const string Cookie = "X-Syki-BearerCookie";

    public static AuthenticationBuilder AddJwtBearerScheme(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var settings = configuration.Auth;

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

            RoleClaimType = SykiClaims.UserRole,
        };

        return builder.AddJwtBearer(Name, options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies[Cookie];
                    if (token.HasValue())
                    {
                        context.Token = token;
                        return Task.CompletedTask;
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }
}
