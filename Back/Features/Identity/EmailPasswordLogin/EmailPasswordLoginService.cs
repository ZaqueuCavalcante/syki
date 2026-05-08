using Syki.Back.Features.Cross.SignIn;
using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Identity.EmailPasswordLogin;

public class EmailPasswordLoginService(
    SignInService service,
    IHttpContextAccessor httpCtx,
    UserManager<SykiUser> userManager) : ICrossService
{
    public async Task<OneOf<EmailPasswordLoginOut, SykiError>> Login(EmailPasswordLoginIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null)
        {
            return new LoginWrongEmailOrPassword();
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return new LoginWrongEmailOrPassword();
        }

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
