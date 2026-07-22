using Estud.Back.Features.Identity.SignIn;

namespace Estud.Back.Features.Identity.TwoFactorSetupLogin;

public static class TwoFactorSetupLoginMapper
{
    extension(SignInOut token)
    {
        public TwoFactorSetupLoginOut ToTwoFactorSetupLoginOut()
        {
            return new()
            {
                Id = token.UserId,
                InstitutionId = token.InstitutionId,
            };
        }
    }
}
