using System.Security.Claims;
using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Institutions;
using Syki.Back.Features.Cross.SignIn;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Syki.Back.Auth.Schemes;

public static class SocialLoginScheme
{
    public const string GoogleScheme = "SocialLogin_Google";

    public static AuthenticationBuilder AddSocialLoginSchemes(
        this AuthenticationBuilder builder,
        IConfiguration configuration)
    {
        var settings = configuration.SocialLogin;

        builder.AddGoogle(GoogleScheme, options =>
        {
            options.ClientId = settings.Google.ClientId;
            options.ClientSecret = settings.Google.ClientSecret;
            options.SignInScheme = SocialTempScheme.Name;
            options.CallbackPath = "/identity/social-login/callback/google";
            options.SaveTokens = false;

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("email");
            options.Scope.Add("profile");

            options.ClaimActions.MapJsonKey("email_verified", "email_verified");

            options.OverrideWith(settings.Google);

            options.Events = new OAuthEvents
            {
                OnRemoteFailure = HandleRemoteFailure,
                OnTicketReceived = HandleTicketReceived,
                OnRedirectToAuthorizationEndpoint = HandleRedirectToAuthorizationEndpoint,
            };
        });

        return builder;
    }

    private static async Task HandleRemoteFailure(RemoteFailureContext context)
    {
        var frontend = context.HttpContext.RequestServices.GetRequiredService<FrontendSettings>();
        context.Response.Redirect($"{frontend.Url}?social_login_error={nameof(SocialLoginFailed)}");
        context.HandleResponse();
    }

    private static Task HandleRedirectToAuthorizationEndpoint(RedirectContext<OAuthOptions> context)
    {
        if (context.Properties.Items.TryGetValue("login_hint", out var loginHint) && loginHint != null)
        {
            context.RedirectUri += $"&login_hint={Uri.EscapeDataString(loginHint)}";
        }
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    }

    private static async Task HandleTicketReceived(TicketReceivedContext context)
    {
        var services = context.HttpContext.RequestServices;
        var ctx = services.GetRequiredService<SykiDbContext>();
        var signInService = services.GetRequiredService<SignInService>();
        var frontendSettings = services.GetRequiredService<FrontendSettings>();
        var userManager = services.GetRequiredService<UserManager<SykiUser>>();

        // 1. Extract email from OAuth claims
        var email = context.Principal?.FindFirst(ClaimTypes.Email)?.Value ?? context.Principal?.FindFirst("email")?.Value;

        if (email.IsEmpty())
        {
            context.Response.Redirect($"{frontendSettings.Url}?social_login_error={nameof(SocialLoginFailed)}");
            context.HandleResponse();
            return;
        }

        email = email!.ToLowerInvariant();

        // 2. Check email_verified claim (Google provides this)
        var emailVerified = context.Principal?.FindFirst("email_verified")?.Value;
        if (emailVerified != "true" && emailVerified != "True")
        {
            context.Response.Redirect($"{frontendSettings.Url}?social_login_error={nameof(SocialLoginEmailNotVerified)}");
            context.HandleResponse();
            return;
        }

        // 3. Check if email domain requires corporate SSO
        if (await ctx.EmailRequiresSsoAsync(email))
        {
            context.Response.Redirect($"{frontendSettings.Url}?social_login_error={nameof(SocialLoginSsoRequired)}");
            context.HandleResponse();
            return;
        }

        // 4. Extract provider info
        var provider = SocialLoginProvider.Google;
        var providerKey = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? context.Principal?.FindFirst("sub")?.Value ?? "";

        // 5. Look up existing social login link by (provider, providerKey)
        var existingLink = await ctx.UserSocialLogins.FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderKey == providerKey);
        if (existingLink != null)
        {
            // Already linked, generate JWT and login
            await signInService.SignIn(existingLink.Email);

            context.Response.Redirect(frontendSettings.BuildUrl("/home"));
            context.HandleResponse();
            return;
        }

        // 6. Look up existing user by email (case-insensitive)
        var existingUser = await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            // Link social account to existing user
            existingUser.EmailConfirmed = true;
            ctx.Add(new UserSocialLogin(existingUser.Id, provider, providerKey, email));
            await ctx.SaveChangesAsync();

            await signInService.SignIn(email);

            context.Response.Redirect(frontendSettings.BuildUrl("/home"));
            context.HandleResponse();
            return;
        }

        // 7. Auto-provision new user on public and web schemes
        var name = ExtractName(context.Principal);
        if (name.IsEmpty()) name = email;

        var directorRole = await ctx.GetDirectorRole();

        var institution = Institution.NewForUserRegister();
        var user = new SykiUser(institution, name, email);
        var userRole = new SykiUserRole(institution, user, directorRole.Id);
        var socialLogin = new UserSocialLogin(user.Id, provider, providerKey, email) { User = user };

        ctx.AddRange(institution, userRole, socialLogin);
        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        await signInService.SignIn(email);

        context.Response.Redirect(frontendSettings.BuildUrl("/home"));
        context.HandleResponse();
    }

    private static string? ExtractName(ClaimsPrincipal? principal)
    {
        if (principal == null) return "User";

        var givenName = principal.FindFirst(ClaimTypes.GivenName)?.Value;
        var surname = principal.FindFirst(ClaimTypes.Surname)?.Value;

        if (givenName.HasValue() && surname.HasValue()) return $"{givenName} {surname}";

        var name = principal.FindFirst(ClaimTypes.Name)?.Value ?? principal.FindFirst("name")?.Value;

        return name;
    }
}
