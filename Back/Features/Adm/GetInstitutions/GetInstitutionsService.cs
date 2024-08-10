namespace Syki.Back.Features.Adm.GetInstitutions;

public class GetInstitutionsService(SykiDbContext ctx) : IAdmService
{
    public async Task<List<InstitutionOut>> Get()
    {
        var institutions = await ctx.Institutions
            .Where(i => i.Id != Guid.Empty)
            .ToListAsync();

        return institutions.ConvertAll(i => i.ToOut());
    }
}
