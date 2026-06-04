namespace Syki.Back.Features.Identity.GetSsoConfiguration;

public class GetSsoConfigurationService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetSsoConfigurationOut?> Get()
    {
        return await ctx.WebSsoConfigurations
            .AsNoTracking()
            .Where(x => x.InstitutionId == ctx.RequestUser.InstitutionId)
            .Select(x => new GetSsoConfigurationOut
            {
                Id = x.PublicId,
                ProviderType = x.ProviderType,
                Authority = x.Authority,
                ClientId = x.ClientId,
                IsActive = x.IsActive,
                RequireSso = x.RequireSso,
                CreatedAt = x.CreatedAt,
            })
            .FirstOrDefaultAsync();
    }
}
