using Estud.Back.Google;
using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Institutions;
using Estud.Back.Features.Cross.SignIn;

namespace Estud.Back.Features.Identity.GoogleOneTapLogin;

public class GoogleOneTapLoginService(
    EstudDbContext ctx,
    SignInService signInService,
    IGoogleService googleService,
    UserManager<EstudUser> userManager,
    SocialLoginSettings socialLoginSettings) : IEstudService
{
    private class Validator : AbstractValidator<GoogleOneTapLoginIn>
    {
        public Validator()
        {
            RuleFor(x => x.Credential).NotEmpty().WithError(GoogleOneTapLoginInvalidToken.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<GoogleOneTapLoginOut, EstudError>> Login(GoogleOneTapLoginIn data)
    {
        if (V.Run(data, out var error)) return error;

        if (!socialLoginSettings.Google.Enabled) return GoogleOneTapLoginDisabled.I;

        var payload = await googleService.ValidateIdTokenAsync(data.Credential, socialLoginSettings.Google.ClientId);
        if (payload == null) return GoogleOneTapLoginInvalidToken.I;

        if (!payload.EmailVerified) return SocialLoginEmailNotVerified.I;

        var email = payload.Email.ToLowerInvariant();
        if (await ctx.EmailRequiresSsoAsync(email)) return SocialLoginSsoRequired.I;

        var provider = SocialLoginProvider.Google;
        var providerKey = payload.Subject;

        // 1. Look up existing social login link by (provider, providerKey)
        var existingLink = await ctx.UserSocialLogins.FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderKey == providerKey);
        if (existingLink != null)
        {
            var jwt = await signInService.SignIn(existingLink.Email);
            return jwt.ToGoogleOneTapLoginOut();
        }

        // 2. Look up existing user by email
        var existingUser = await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            existingUser.ConfirmEmail();
            ctx.Add(new UserSocialLogin(existingUser.Id, provider, providerKey, email));
            await ctx.SaveChangesAsync();
            var jwt = await signInService.SignIn(email);
            return jwt.ToGoogleOneTapLoginOut();
        }

        // 3. Auto-provision new user on public and web schemes
        var name = payload.Name;
        if (name.IsEmpty()) name = email;

        var institution = Institution.NewForUserRegister();
        var directorRole = institution.GetDirectorRole();

        var user = new EstudUser(institution, name, email);
        var userRole = new EstudUserRole(institution, user, directorRole);
        var socialLogin = new UserSocialLogin(user.Id, provider, providerKey, email) { User = user };

        ctx.AddRange(institution, userRole, socialLogin);
        await userManager.CreateAsync(user, $"Estud@{Guid.NewGuid()}");

        var jwtResult = await signInService.SignIn(email);
        return jwtResult.ToGoogleOneTapLoginOut();
    }
}
