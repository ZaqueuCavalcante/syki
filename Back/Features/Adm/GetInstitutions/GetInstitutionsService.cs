namespace Syki.Back.GetInstitutions;

public class GetInstitutionsService(SykiDbContext ctx)
{
    public async Task<List<InstitutionOut>> Get()
    {
        var institutions = await ctx.Institutions
            .Where(i => i.Id != Guid.Empty)
            .ToListAsync();

        return institutions.ConvertAll(i => i.ToOut());
    }
}
