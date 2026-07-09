namespace Estud.Back.Features.Adm.GetFeatureFlags;

public class GetFeatureFlagsService(EstudDbContext ctx, FeaturesSettings settings) : IEstudService
{
    public async Task<GetFeatureFlagsOut> Get()
    {
        var featureFlags = await ctx.FeatureFlags.AsNoTracking()
            .Where(i => i.Id == Guid.Empty)
            .FirstOrDefaultAsync();
        
        featureFlags ??= new()
        {
            CrossLogin = settings.CrossLogin,
        };

        return featureFlags.ToOut();
    }
}
