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
            features.Settings.SkipUserRegister = data.SkipUserRegister;
            features.Settings.CrossLogin = data.CrossLogin;
        }

        await ctx.SaveChangesAsync();

        settings.SkipUserRegister = features.Settings.SkipUserRegister;
        settings.CrossLogin = features.Settings.CrossLogin;
    }
}
