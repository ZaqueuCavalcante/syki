namespace Syki.Back.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddRouting(options => options.LowercaseUrls = true);
    }
}
