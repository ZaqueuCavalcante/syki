using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public static class DaemonFactoryExtensions
{
    public static SykiDbContext GetDbContext(this DaemonFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SykiDbContext>();
    }

    public static async Task AwaitEventsProcessing(this DaemonFactory factory)
    {
        await using var ctx = factory.GetDbContext();
        var count = 0;
        while (true)
        {
            if (count == 2) break;

            var events = await ctx.DomainEvents.CountAsync(x => x.ProcessedAt == null);
            if (events == 0) break;
            await Task.Delay(500);
            count ++;
        }
    }

    public static async Task AwaitCommandsProcessing(this DaemonFactory factory)
    {
        await using var ctx = factory.GetDbContext();
        var count = 0;
        while (true)
        {
            if (count == 2) break;

            var commands = await ctx.Commands.CountAsync(x => x.ProcessedAt == null);
            if (commands == 0) break;
            await Task.Delay(500);
            count ++;
        }
    }

    public static T GetService<T>(this DaemonFactory factory) where T : notnull
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}
