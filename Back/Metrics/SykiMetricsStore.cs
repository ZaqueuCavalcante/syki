using System.Collections.Concurrent;

namespace Syki.Back.Metrics;

public static class SykiMetricsStore
{
    public static ConcurrentDictionary<string, ConcurrentDictionary<string, int>> Requests = new();
}
