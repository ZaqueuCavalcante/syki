using Syki.Back.CreateUser;
using Syki.Shared.GetMfaKey;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.GetMfaKey;

public class GetMfaKeyService(UserManager<SykiUser> userManager)
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
