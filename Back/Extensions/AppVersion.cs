using System.Reflection;

namespace Estud.Back.Extensions;

public static class AppVersion
{
    public static string Value { get; } = Assembly.GetEntryAssembly()?
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion
        .Split('+').Last()[..7] ?? "unknown";
}
