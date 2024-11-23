namespace Syki.Back.Features.Adm.SetupFeatureFlags;

public class SetupFeatureFlagsService(SykiDbContext ctx, FeaturesSettings settings) : IAdmService
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
            features.SkipUserRegister = data.SkipUserRegister;
            features.CrossLogin = data.CrossLogin;
        }

        await ctx.SaveChangesAsync();

        settings.SkipUserRegister = features.SkipUserRegister;
        settings.CrossLogin = features.CrossLogin;
    }
}
