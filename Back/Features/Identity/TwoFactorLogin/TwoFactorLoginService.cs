using System.Security.Claims;
using Syki.Back.Domain.Identity;
using Syki.Back.Features.Cross.SignIn;
using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.Features.Identity.TwoFactorLogin;

public class TwoFactorLoginService(
    SignInService service,
    IHttpContextAccessor httpCtx,
    UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<OneOf<TwoFactorLoginOut, SykiError>> Login(TwoFactorLoginIn data)
    {
        var authResult = await httpCtx.HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
        var userId = authResult.Principal?.FindFirstValue(ClaimTypes.Name);
        if (userId == null) return InvalidTwoFactorToken.I;

        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return InvalidTwoFactorToken.I;

        if (await userManager.IsLockedOutAsync(user)) return InvalidTwoFactorToken.I;

        var token = data.Token!.OnlyNumbers();
        var tokenProvider = userManager.Options.Tokens.AuthenticatorTokenProvider;
        var isValid = await userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token);

        if (!isValid)
        {
            await userManager.AccessFailedAsync(user);
            return InvalidTwoFactorToken.I;
        }

        await userManager.ResetAccessFailedCountAsync(user);

        // Clear the 2FA cookie (we use JWT for auth)
        await httpCtx.HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);

        var jwtResult = await service.SignIn(user.Email);

        return jwtResult.ToTwoFactorLoginOut();
    }
}
