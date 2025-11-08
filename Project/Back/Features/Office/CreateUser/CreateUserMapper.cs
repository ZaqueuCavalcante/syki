using Exato.Shared.Features.Office.CreateUser;
using Exato.Shared.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Office.CreateUser;

public static class CreateUserMapper
{
    extension(CreateUserIn user)
    {
        public CreateExatoUserIn ToCreateExatoUserIn()
        {
            return new()
            {
                Name = user.Name,
                RoleId = user.RoleId,
                Email = user.Email,
                Password = $"Exato@{Guid.NewGuid()}",
                OrganizationId = user.OrganizationId,
            };
        }
    }

    extension(CreateExatoUserOut user)
    {
        public CreateUserOut ToCreateUserOut()
        {
            return new()
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                Email = user.Email,
                OrganizationId = user.OrganizationId,
            };
        }
    }
}
