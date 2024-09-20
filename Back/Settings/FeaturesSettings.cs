namespace Syki.Back.Settings;

public class FeaturesSettings
{
    public bool SkipUserRegister { get; set; }
    public bool CrossLogin { get; set; }

    public FeaturesSettings(IConfiguration configuration)
    {
        configuration.GetSection("Features").Bind(this);
    }
}
