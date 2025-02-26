namespace Syki.Back.Features.Cross.SetupFeatureFlags;

public class FeatureFlags
{
    public Guid Id { get; set; }
    public bool CrossLogin { get; set; }

    public FeatureFlags() {}

    public FeatureFlags(SetupFeatureFlagsIn data)
    {
        Id = Guid.Empty;
        CrossLogin = data.CrossLogin;
    }

    public GetFeatureFlagsOut ToOut()
    {
        return new()
        {
            CrossLogin = CrossLogin,
        };
    }
}
