namespace Estud.Back.Features.Adm.SetupFeatureFlags;

public class SetupFeatureFlagsService(EstudDbContext ctx, FeaturesSettings settings) : IEstudService
{
    public async Task Setup(SetupFeatureFlagsIn data)
    {
        var features = await ctx.FeatureFlags
            .Where(f => f.Id == Guid.Empty)
            .FirstOrDefaultAsync();

        if (features == null)
        {
            features ??= new(data);
            ctx.Add(features);
        }
        else
        {
            features.CrossLogin = data.CrossLogin;
        }

        await ctx.SaveChangesAsync();

        settings.CrossLogin = features.CrossLogin;
    }
}
