using Estud.Back.Domain.Identity;
using Estud.Back.Features.Cross.SignIn;

namespace Estud.Back.Features.Identity.EmailPasswordLogin;

public class EmailPasswordLoginService(
    SignInService service,
    IHttpContextAccessor httpCtx,
    UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<OneOf<EmailPasswordLoginOut, EstudError>> Login(EmailPasswordLoginIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null) return new LoginWrongEmailOrPassword();

        if (await userManager.IsLockedOutAsync(user)) return new LoginUserLockedOut();

        var isValidPassword = await userManager.CheckPasswordAsync(user, data.Password);
        if (!isValidPassword)
        {
            await userManager.AccessFailedAsync(user);
            return new LoginWrongEmailOrPassword();
        }

        // Reset failed access count on successful login
        await userManager.ResetAccessFailedCountAsync(user);

        if (user.TwoFactorEnabled)
        {
            await httpCtx.HttpContext.SignInTwoFactorUserIdSchemeAsync(user.Id);
            return new LoginRequiresTwoFactor();
        }

        var signInResult = await service.SignIn(data.Email);
        return signInResult.ToEmailPasswordLoginOut();
    }
}
