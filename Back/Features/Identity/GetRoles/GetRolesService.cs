namespace Estud.Back.Features.Identity.GetRoles;

public class GetRolesService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetRolesOut> Get()
    {
        var roles = await ctx.Roles.AsNoTracking()
            .Where(r => r.OwnerId == ctx.RequestUser.InstitutionId)
            .OrderBy(r => r.Name)
            .ToListAsync();

        return new GetRolesOut
        {
            Total = roles.Count,
            Items = roles.ConvertAll(r => new GetRolesItemOut
            {
                Id = r.Id,
                Name = r.Name!,
                BaseType = r.BaseType,
                Description = r.Description,
                Permissions = r.Permissions.Count,
            }),
        };
    }
}
