namespace Syki.Back.Features.Identity.GetRole;

public class GetRoleService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetRoleOut, SykiError>> Get(int id)
    {
        var role = await ctx.Roles.AsNoTracking()
            .FirstOrDefaultAsync(r => r.OwnerId == ctx.RequestUser.InstitutionId && r.Id == id);
        if (role == null) return RoleNotFound.I;

        return new GetRoleOut
        {
            Id = role.Id,
            Name = role.Name!,
            Description = role.Description,
            BaseType = role.BaseType,
            Permissions = role.Permissions,
        };
    }
}
