using Syki.Back.Auth.Permissions;

namespace Syki.Back.Features.Identity.GetPermissions;

public class GetPermissionsService : ISykiService
{
    public GetPermissionsOut Get()
    {
        return new GetPermissionsOut
        {
            Items = SykiPermissions.Permissions.ConvertAll(p => new GetPermissionsItemOut
            {
                Id = p.Id,
                Name = p.Name,
                AllowedTypes = p.AllowedTypes,
            }),
        };
    }
}
