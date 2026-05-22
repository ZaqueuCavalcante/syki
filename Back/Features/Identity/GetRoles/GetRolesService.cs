namespace Syki.Back.Features.Identity.GetRoles;

public class GetRolesService(SykiDbContext ctx) : ISykiService
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
                Description = r.Description,
                Permissions = r.Permissions.Count,
            }),
        };
    }
}
