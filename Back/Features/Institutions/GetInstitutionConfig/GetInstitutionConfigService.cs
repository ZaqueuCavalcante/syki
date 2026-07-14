namespace Estud.Back.Features.Institutions.GetInstitutionConfig;

public class GetInstitutionConfigService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetInstitutionConfigOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var config = await ctx.InstitutionConfigs.AsNoTracking()
            .FirstAsync(x => x.InstitutionId == institutionId);

        return config.ToGetInstitutionConfigOut();
    }
}
