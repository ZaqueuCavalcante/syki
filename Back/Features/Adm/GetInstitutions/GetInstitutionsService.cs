namespace Syki.Back.Features.Adm.GetInstitutions;

public class GetInstitutionsService(SykiDbContext ctx) : IAdmService
{
    public async Task<List<InstitutionOut>> Get()
    {
        var institutions = await ctx.Institutions
            .Include(x => x.Configs)
            .Where(i => i.Id != Guid.Empty)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return institutions.ConvertAll(i => i.ToOut());
    }
}
