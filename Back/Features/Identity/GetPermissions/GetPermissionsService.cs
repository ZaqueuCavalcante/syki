using Estud.Back.Auth.Permissions;

namespace Estud.Back.Features.Identity.GetPermissions;

public class GetPermissionsService : IEstudService
{
    public GetPermissionsOut Get()
    {
        return new GetPermissionsOut
        {
            Items = EstudPermissions.Permissions.ConvertAll(p => new GetPermissionsItemOut
            {
                Id = p.Id,
                Name = p.Name,
                AllowedTypes = p.AllowedTypes,
            }),
        };
    }
}
