using Estud.Back.Features.Identity.SignIn;

namespace Estud.Back.Features.Identity.TwoFactorLogin;

public static class TwoFactorLoginMapper
{
    extension(SignInOut token)
    {
        public TwoFactorLoginOut ToTwoFactorLoginOut()
        {
            return new()
            {
                Id = token.UserId,
                InstitutionId = token.InstitutionId,
            };
        }
    }
}
