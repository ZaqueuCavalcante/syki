namespace Estud.Back.Settings;

public class JobsSettings : SettingsBase
{
    public int CommandsPollingIntervalInSeconds { get; set; } = 60;
    public int DomainEventsPollingIntervalInSeconds { get; set; } = 60;
    public int WebhookCallsPollingIntervalInSeconds { get; set; } = 60;

    public JobsSettings(IConfiguration configuration)
    {
        configuration.GetSection("Jobs").Bind(this);

        RequirePositive(CommandsPollingIntervalInSeconds);
        RequirePositive(DomainEventsPollingIntervalInSeconds);
        RequirePositive(WebhookCallsPollingIntervalInSeconds);
    }
}

public static class JobsSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public JobsSettings Jobs => new(configuration);
    }
}
