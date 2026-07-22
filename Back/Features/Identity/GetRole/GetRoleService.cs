namespace Estud.Back.Features.Identity.GetRole;

public class GetRoleService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetRoleOut, EstudError>> Get(int id)
    {
        var role = await ctx.Roles.AsNoTracking()
            .FirstOrDefaultAsync(r => r.InstitutionId == ctx.RequestUser.InstitutionId && r.Id == id);
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
