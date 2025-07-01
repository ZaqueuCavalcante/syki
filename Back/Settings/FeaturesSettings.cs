namespace Syki.Back.Settings;

public class FeaturesSettings
{
    public bool CrossLogin { get; set; }

    public FeaturesSettings() {}

    public FeaturesSettings(IConfiguration configuration)
    {
        configuration.GetSection("Features").Bind(this);
    }
}
