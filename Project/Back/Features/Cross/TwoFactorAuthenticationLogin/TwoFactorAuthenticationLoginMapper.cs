using Exato.Shared.Features.Cross.GenerateJWT;
using Exato.Shared.Features.Cross.TwoFactorAuthenticationLogin;

namespace Exato.Back.Features.Cross.TwoFactorAuthenticationLogin;

public static class TwoFactorAuthenticationLoginMapper
{
    extension(GenerateJWTOut token)
    {
        public TwoFactorAuthenticationLoginOut ToTwoFactorAuthenticationLoginOut()
        {
            return new()
            {
                Id = token.Id,
                Name = token.Name,
                Email = token.Email,
                Role = token.Role,
                Features = token.Features,
            };
        }
    }
}
