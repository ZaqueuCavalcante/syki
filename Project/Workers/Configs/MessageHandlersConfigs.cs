namespace Exato.Workers.Configs;

public static class MessageHandlersConfigs
{
    public static void AddWorkersMessageHandlersConfigs(this WebApplicationBuilder builder)
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
