namespace Syki.Shared;

public class GetFeatureFlagsOut
{
    public bool SkipUserRegister { get; set; }
    public bool CrossLogin { get; set; }
}

public class FeatureFlagOut
{
    public string Name { get; set; }
    public bool Active { get; set; }
}
