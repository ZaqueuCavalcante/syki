namespace Syki.Back.CreateInstitution;

public class CreateInstitutionService(SykiDbContext ctx)
{
    public async Task<FaculdadeOut> Create(FaculdadeIn data)
    {
        var institution = new Faculdade(data.Nome);
        ctx.Add(institution);

        await ctx.SaveChangesAsync();

        return institution.ToOut();
    }
}
