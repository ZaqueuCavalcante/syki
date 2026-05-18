using Syki.Back.Features.Identity.SignIn;

namespace Syki.Back.Features.Identity.EmailPasswordLogin;

public static class EmailPasswordLoginMapper
{
    extension(SignInOut token)
    {
        public EmailPasswordLoginOut ToEmailPasswordLoginOut()
        {
            return new()
            {
                UserId = token.UserId,
                InstitutionId = token.InstitutionId,
            };
        }
    }
}
