namespace Syki.Back.Configs;

public static class CommandConfigs
{
    private static readonly Dictionary<string, Type> _commandTypes = [];

    public static void AddCommandConfigs(this WebApplicationBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName!.StartsWith("Back"))
            .ToList();

        var commandTypes = assemblies.SelectMany(s => s.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(ICommand)) && !t.IsInterface)
            .ToList();

        foreach (var type in commandTypes)
        {
            _commandTypes[type.Name] = type;
        }

        var handlerTypes = assemblies.SelectMany(s => s.GetTypes())
            .Where(t => !t.IsInterface && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            .ToList();

        var handledCommandTypes = new HashSet<Type>();

        foreach (var type in handlerTypes)
        {
            var handlerInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

            handledCommandTypes.Add(handlerInterface.GetGenericArguments()[0]);
            builder.Services.AddTransient(handlerInterface, type);
        }

        var commandsWithoutHandler = commandTypes.Except(handledCommandTypes).ToList();
        if (commandsWithoutHandler.Count > 0)
        {
            var names = string.Join(", ", commandsWithoutHandler.Select(t => t.Name));
            throw new InvalidOperationException($"Commands without a registered ICommandHandler<T>: {names}");
        }
    }

    public static Type GetCommandType(string typeName)
    {
        return _commandTypes.TryGetValue(typeName, out var type) ? type
            : throw new InvalidOperationException($"Command type '{typeName}' not found in registry.");
    }
}
