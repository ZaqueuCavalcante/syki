namespace Syki.Back.Configs;

public static class TestingConfigs
{
    public static void UseTestingMetrics(this IApplicationBuilder app)
    {
        if (!Env.IsTesting()) return;

        app.UseMiddleware<MetricsMiddleware>();
    }
}
