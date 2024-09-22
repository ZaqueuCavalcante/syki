namespace Syki.Back.Features.Adm.GetFeatureFlags;

public class GetFeatureFlagsService(SykiDbContext ctx, FeaturesSettings settings) : IAdmService
{
    public async Task<GetFeatureFlagsOut> Get()
    {
        var featureFlags = await ctx.FeatureFlags.AsNoTracking()
            .Where(i => i.Id == Guid.Empty)
            .FirstOrDefaultAsync();
        
        featureFlags ??= new() { Settings = settings };

        return featureFlags.ToOut();
    }
}
