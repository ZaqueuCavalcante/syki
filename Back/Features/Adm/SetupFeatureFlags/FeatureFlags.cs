namespace Syki.Back.Features.Cross.SetupFeatureFlags;

public class FeatureFlags
{
    public Guid Id { get; set; }
    public FeaturesSettings Settings { get; set; }

    public FeatureFlags() {}

    public FeatureFlags(SetupFeatureFlagsIn data)
    {
        Id = Guid.Empty;
        Settings = new()
        {
            SkipUserRegister = data.SkipUserRegister,
            CrossLogin = data.CrossLogin,
        };
    }

    public GetFeatureFlagsOut ToOut()
    {
        return new()
        {
            SkipUserRegister = Settings.SkipUserRegister,
            CrossLogin = Settings.CrossLogin,
        };
    }
}
