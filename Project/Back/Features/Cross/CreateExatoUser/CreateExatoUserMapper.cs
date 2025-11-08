using Exato.Shared.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Cross.CreateExatoUser;

public static class CreateExatoUserMapper
{
    extension(ExatoUser user)
    {
        public CreateExatoUserOut ToCreateExatoUserOut(string role)
        {
            return new()
            {
                Role = role,
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                OrganizationId = user.OrganizationId,
            };
        }
    }
}
