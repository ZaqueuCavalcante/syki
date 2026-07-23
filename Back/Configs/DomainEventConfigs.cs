namespace Estud.Back.Configs;

public static class DomainEventConfigs
{
    private static readonly Dictionary<string, Type> _domainEventTypes = [];

    public static void AddDomainEventConfigs(this WebApplicationBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName!.StartsWith("Back"))
            .ToList();

        var domainEventTypes = assemblies.SelectMany(s => s.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(IDomainEvent)) && !t.IsInterface)
            .ToList();

        foreach (var type in domainEventTypes)
        {
            _domainEventTypes[type.FullName!] = type;
        }

        var handlerTypes = assemblies.SelectMany(s => s.GetTypes())
            .Where(t => !t.IsInterface && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
            .ToList();

        foreach (var type in handlerTypes)
        {
            foreach (var handlerInterface in type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
            {
                builder.Services.AddTransient(handlerInterface, type);
            }
        }
    }

    public static Type GetDomainEventType(string typeName)
    {
        return _domainEventTypes.TryGetValue(typeName, out var type) ? type
            : throw new InvalidOperationException($"Domain event type '{typeName}' not found in registry.");
    }
}
