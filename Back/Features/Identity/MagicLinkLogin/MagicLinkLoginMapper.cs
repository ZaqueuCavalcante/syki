using Syki.Back.Features.Identity.SignIn;

namespace Syki.Back.Features.Identity.MagicLinkLogin;

public static class MagicLinkLoginMapper
{
    extension(SignInOut token)
    {
        public MagicLinkLoginOut ToMagicLinkLoginOut()
        {
            return new()
            {
                UserId = token.UserId,
                InstitutionId = token.InstitutionId,
            };
        }
    }
}
