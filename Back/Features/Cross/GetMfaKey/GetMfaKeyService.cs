using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.GetMfaKey;

public class GetMfaKeyService(UserManager<SykiUser> userManager) : ICrossService
{
    public async Task<GetMfaKeyOut> Get(Guid userId)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId);

        var key = await userManager.GetAuthenticatorKeyAsync(user);

        if (key == null)
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            key = await userManager.GetAuthenticatorKeyAsync(user);
        }

        return new GetMfaKeyOut { Key = key! };
    }
}
