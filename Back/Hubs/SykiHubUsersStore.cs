using System.Collections.Concurrent;

namespace Syki.Back.Hubs;

public static class SykiHubUsersStore
{
    public static ConcurrentDictionary<int, List<string>> Users = new();
}
