using Syki.Back.Metrics;
using System.Collections.Concurrent;

namespace Syki.Back.Middlewares;

public class MetricsMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var method = context.Request.Method;
            var endpoint = context.Request.Path.ToString();
            var status = context.Response.StatusCode.ToString();

            SykiMetricsStore.Requests.AddOrUpdate(
                $"{method} {endpoint}",
                _ =>
                {
                    var dict = new ConcurrentDictionary<string, int>();
                    dict.TryAdd("Total", 1);
                    dict.TryAdd(method, 1);
                    dict.TryAdd(status, 1);
                    return dict;
                },
                (key, currentValue) =>
                {
                    currentValue.AddOrUpdate("Total", 1, (_, oldValue) => oldValue + 1);
                    currentValue.AddOrUpdate(method, 1, (_, oldValue) => oldValue + 1);
                    currentValue.AddOrUpdate(status, 1, (_, oldValue) => oldValue + 1);
                    return currentValue;
                });

            return Task.CompletedTask;
        });

        await next(context);
    }
}
