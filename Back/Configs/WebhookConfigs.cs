using Estud.Back.Webhooks;

namespace Estud.Back.Configs;

public static class WebhookConfigs
{
    private static readonly Dictionary<string, Type> _eventTypes = [];

    public static void AddWebhookEventConfigs(this WebApplicationBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName!.StartsWith("Back"))
            .ToList();

        var eventTypes = assemblies.SelectMany(s => s.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(IWebhookEvent)) && !t.IsInterface)
            .ToList();

        foreach (var type in eventTypes)
        {
            var attr = type.GetCustomAttributes(typeof(WebhookEventTypeAttribute), false)
                .Cast<WebhookEventTypeAttribute>()
                .FirstOrDefault();

            if (attr == null) throw new InvalidOperationException($"IWebhookEvent '{type.Name}' is missing [WebhookEventType] attribute.");

            _eventTypes[attr.Type] = type;
        }

        var handlerTypes = assemblies.SelectMany(s => s.GetTypes())
            .Where(t => !t.IsInterface && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IWebhookEventHandler<>)))
            .ToList();

        var handledEventTypes = new HashSet<Type>();

        foreach (var type in handlerTypes)
        {
            var handlerInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IWebhookEventHandler<>));

            handledEventTypes.Add(handlerInterface.GetGenericArguments()[0]);
            builder.Services.AddTransient(handlerInterface, type);
        }

        var eventsWithoutHandler = eventTypes.Except(handledEventTypes).ToList();
        if (eventsWithoutHandler.Count > 0)
        {
            var names = string.Join(", ", eventsWithoutHandler.Select(t => t.Name));
            throw new InvalidOperationException($"Webhook events without a registered IWebhookEventHandler<T>: {names}");
        }
    }

    public static bool TryGetEventType(string type, out Type eventType)
    {
        return _eventTypes.TryGetValue(type, out eventType!);
    }
}
