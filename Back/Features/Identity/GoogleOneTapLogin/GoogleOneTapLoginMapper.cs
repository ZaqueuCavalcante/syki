using Syki.Back.Features.Identity.SignIn;

namespace Syki.Back.Features.Identity.GoogleOneTapLogin;

public static class GoogleOneTapLoginMapper
{
    extension(SignInOut token)
    {
        public GoogleOneTapLoginOut ToGoogleOneTapLoginOut()
        {
            return new()
            {
                UserId = token.UserId,
                InstitutionId = token.InstitutionId,
                Permissions = token.Permissions,
            };
        }
    }
}
