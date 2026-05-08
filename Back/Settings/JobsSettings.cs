namespace Syki.Back.Settings;

public class JobsSettings
{
    public int CommandsPollingIntervalInSeconds { get; set; } = 60;

    public JobsSettings(IConfiguration configuration)
    {
        configuration.GetSection("Jobs").Bind(this);
    }
}

public static class JobsSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public JobsSettings Jobs => new(configuration);
    }
}
