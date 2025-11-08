using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Exato.Tests.Integration.Base;

public static class WorkersFactoryExtensions
{
    public static HttpClient GetClient(this WorkersFactory factory)
    {
        return factory.CreateClient();
    }

    public static BackDbContext GetBackDbContext(this WorkersFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<BackDbContext>();
    }

    public static async Task AwaitEventsProcessing(this WorkersFactory factory)
    {
        await using var ctx = factory.GetBackDbContext();

        var count = 0;
        while (true)
        {
            if (count == 5) break;

            var events = await ctx.ExatoDomainEvents.CountAsync(x => x.ProcessedAt == null);
            if (events == 0) break;
            await Task.Delay(500);
            count++;
        }
    }

    public static async Task AwaitCommandsProcessing(this WorkersFactory factory)
    {
        await using var ctx = factory.GetBackDbContext();

        var count = 0;
        while (true)
        {
            if (count == 5) break;

            var commands = await ctx.ExatoCommands.CountAsync(x => x.ProcessedAt == null);
            if (commands == 0) break;
            await Task.Delay(500);
            count ++;
        }
    }
}
