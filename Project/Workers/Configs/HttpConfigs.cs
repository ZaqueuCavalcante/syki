namespace Exato.Workers.Configs;

public static class HttpConfigs
{
    public static void AddWorkersHttpConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
    }
}
