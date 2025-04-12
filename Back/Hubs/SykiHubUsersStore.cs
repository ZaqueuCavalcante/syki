using System.Collections.Concurrent;

namespace Syki.Back.Hubs;

public class SykiHubUsersStore
{
    public static HashSet<Guid> ConnectedIds = [];
    public static ConcurrentDictionary<Guid, List<string>> Users = new();
}
