namespace Syki.Daemon.Configs;

public static class HandlersConfigs
{
    public static void AddDaemonHandlersConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddHandlers("CommandHandler");
        builder.Services.AddHandlers("DomainEventHandler");
    }

    private static void AddHandlers(this IServiceCollection services, string handlerName)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName.StartsWith("Back"))
            .SelectMany(s => s.GetTypes())
            .Where(t => t.FullName.EndsWith(handlerName) && !t.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            services.AddTransient(type);
        }
    }
}
