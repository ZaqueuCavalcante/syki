using Quartz;

namespace Estud.Back.Middlewares;

public class BackgroundProcessorsTriggerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, EstudDbContext ctx, ISchedulerFactory factory)
    {
        await next(context);

        if (!ctx.HasPendingCommands && !ctx.HasPendingDomainEvents) return;

        var scheduler = await factory.GetScheduler();

        if (ctx.HasPendingCommands) await scheduler.TriggerCommandsProcessorJob();
        if (ctx.HasPendingDomainEvents) await scheduler.TriggerDomainEventsProcessorJob();
    }
}
