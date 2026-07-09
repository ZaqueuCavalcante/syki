using Dapper;
using Npgsql;
using System.Security.Claims;
using Estud.Back.Auth.Managers;
using Estud.Back.Domain.Identity;
using Microsoft.Extensions.Options;
using Estud.Back.Features.Cross.SignIn;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estud.Back.Auth.Schemes;

public static class SsoOidcScheme
{
    public const string Prefix = "OIDC_";

    public static AuthenticationBuilder AddSsoOpenIdConnectScheme(this AuthenticationBuilder builder)
    {
        builder.Services.AddSingleton<SsoSchemeManager>();
        builder.Services.AddSingleton<SsoEncryptionManager>();

        builder.Services.TryAddTransient<OpenIdConnectHandler>();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<OpenIdConnectOptions>, OpenIdConnectPostConfigureOptions>());

        return builder;
    }

    public static void RegisterActiveSsoSchemes(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var ssoSchemeManager = scope.ServiceProvider.GetRequiredService<SsoSchemeManager>();

        const string sql = @"
            SELECT
                id,
                authority,
                client_id,
                external_id,
                client_secret,
                updated_at
            FROM
                estud.sso_configurations
            WHERE
                is_active = true
        ";

        using var connection = new NpgsqlConnection(configuration.Database.ConnectionString);
        var configs = connection.Query<SsoConfiguration>(sql).ToList();

        foreach (var config in configs)
        {
            ssoSchemeManager.RegisterScheme(config);
        }
    }

    public static void ConfigureSsoSchemeOptions(OpenIdConnectOptions options, SsoConfiguration config)
    {
        options.Authority = config.Authority;
        options.ClientId = config.ClientId;
        options.ClientSecret = config.ClientSecret;
        options.ResponseType = "code";
        options.UsePkce = true;
        options.SaveTokens = false;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SignInScheme = SsoTempScheme.Name;
        options.CallbackPath = $"/identity/sso/callback/{config.PublicId}";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("email");
        options.Scope.Add("profile");

        options.Events = new OpenIdConnectEvents
        {
            OnRemoteFailure = HandleRemoteFailure,
            OnTicketReceived = HandleTicketReceived,
            OnAuthorizationCodeReceived = HandleAuthorizationCodeReceived,
            OnRedirectToIdentityProvider = HandleRedirectToIdentityProvider,
        };
    }

    private static async Task HandleRemoteFailure(RemoteFailureContext context)
    {
        var frontendSettings = context.HttpContext.RequestServices.GetRequiredService<FrontendSettings>();
        var ctx = context.HttpContext.RequestServices.GetRequiredService<EstudDbContext>();

        context.Response.Redirect($"{frontendSettings.Url}?sso_error={nameof(SsoAuthenticationFailed)}");
        context.HandleResponse();
    }

    private static async Task HandleTicketReceived(TicketReceivedContext context)
    {
        var services = context.HttpContext.RequestServices;
        var ctx = services.GetRequiredService<EstudDbContext>();
        var authSettings = services.GetRequiredService<AuthSettings>();
        var signInService = services.GetRequiredService<SignInService>();
        var frontendSettings = services.GetRequiredService<FrontendSettings>();

        // Extract email from OIDC claims
        var email = context.Principal?.FindFirst(ClaimTypes.Email)?.Value ?? context.Principal?.FindFirst("email")?.Value;

        if (email.IsEmpty())
        {
            context.Response.Redirect($"{frontendSettings.Url}?sso_error={nameof(SsoAuthenticationFailed)}");
            context.HandleResponse();
            return;
        }

        var domain = email!.Split('@').Last().ToLowerInvariant();

        // Load SSO configuration for this domain
        var ssoConfigId = await ctx.WebSsoAllowedDomains.Where(x => x.Domain == domain).Select(x => x.SsoConfigurationId).FirstOrDefaultAsync();
        var ssoConfig = await ctx.WebSsoConfigurations.Where(x => x.Id == ssoConfigId && x.IsActive).FirstOrDefaultAsync();
        if (ssoConfig == null)
        {
            context.Response.Redirect($"{frontendSettings.Url}?sso_error={nameof(SsoNotConfiguredForDomain)}");
            context.HandleResponse();
            return;
        }

        // Look up user by email
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            context.Response.Redirect($"{frontendSettings.Url}?sso_error={nameof(SsoLoginUserNotFound)}");
            context.HandleResponse();
            return;
        }

        // Set EmailConfirmed if the IdP confirmed the email is verified
        var emailVerifiedClaim = context.Principal?.FindFirst("email_verified")?.Value;
        if (emailVerifiedClaim is "true" or "True")
        {
            user.EmailConfirmed = true;
        }

        // Generate JWT and set cookie
        await signInService.SignIn(email);

        context.Response.Redirect(frontendSettings.BuildUrl("/home"));
        context.HandleResponse();
    }

    /// <summary>
    /// Fires before the token exchange with the OIDC provider.
    /// Refreshes the ClientSecret if the in-memory scheme is stale (e.g., config updated on another instance).
    /// </summary>
    private static async Task HandleAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
    {
        var schemeName = context.Scheme.Name;
        var publicId = Guid.Parse(schemeName[Prefix.Length..]);

        var services = context.HttpContext.RequestServices;
        var ctx = services.GetRequiredService<EstudDbContext>();
        var ssoSchemeManager = services.GetRequiredService<SsoSchemeManager>();

        var config = await ctx.GetActiveSsoConfigForSchemeAsync(publicId);
        if (config == null) return;

        if (ssoSchemeManager.IsStale(schemeName, config.UpdatedAt))
        {
            var encryption = services.GetRequiredService<SsoEncryptionManager>();
            context.TokenEndpointRequest!.ClientSecret = encryption.Decrypt(config.ClientSecret);
            ssoSchemeManager.RegisterScheme(config);
        }
    }

    private static Task HandleRedirectToIdentityProvider(RedirectContext context)
    {
        if (context.Properties.Items.TryGetValue("login_hint", out var loginHint))
        {
            context.ProtocolMessage.LoginHint = loginHint;
        }
        return Task.CompletedTask;
    }
}
