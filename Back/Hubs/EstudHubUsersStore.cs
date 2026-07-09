using System.Collections.Concurrent;

namespace Estud.Back.Hubs;

public static class EstudHubUsersStore
{
    public static ConcurrentDictionary<int, List<string>> Users = new();
}
