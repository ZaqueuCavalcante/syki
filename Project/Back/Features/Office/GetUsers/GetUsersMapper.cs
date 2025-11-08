using Exato.Shared.Features.Office.GetUsers;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Office.GetUsers;

public static class GetUsersMapper
{
    extension(ExatoUser user)
    {
        public GetUsersItemOut ToGetUsersItemOut()
        {
            return new()
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                Email = user.Email ?? "-",
                CreatedAt = user.CreatedAt, 
                TwoFactorEnabled = user.TwoFactorEnabled,
            };
        }
    }
}
