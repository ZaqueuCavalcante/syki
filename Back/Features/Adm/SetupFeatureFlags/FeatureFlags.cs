namespace Syki.Back.Features.Cross.SetupFeatureFlags;

public class FeatureFlags
{
    public Guid Id { get; set; }
    public bool CrossLogin { get; set; }
    public bool SkipUserRegister { get; set; }

    public FeatureFlags() {}

    public FeatureFlags(SetupFeatureFlagsIn data)
    {
        Id = Guid.Empty;
        CrossLogin = data.CrossLogin;
        SkipUserRegister = data.SkipUserRegister;
    }

    public GetFeatureFlagsOut ToOut()
    {
        return new()
        {
            CrossLogin = CrossLogin,
            SkipUserRegister = SkipUserRegister,
        };
    }
}
