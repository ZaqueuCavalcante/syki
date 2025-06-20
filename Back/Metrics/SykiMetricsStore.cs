using System.Collections.Concurrent;

namespace Syki.Back.Metrics;

public static class SykiMetricsStore
{
    // GET /academic/disciplines
    public static ConcurrentDictionary<string, ConcurrentDictionary<string, int>> Requests = new();
}
