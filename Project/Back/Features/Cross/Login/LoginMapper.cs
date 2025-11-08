using Exato.Shared.Features.Cross.Login;
using Exato.Shared.Features.Cross.GenerateJWT;

namespace Exato.Back.Features.Cross.Login;

public static class LoginMapper
{
    extension(GenerateJWTOut token)
    {
        public LoginOut ToLoginOut()
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
