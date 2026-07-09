using Quartz;

namespace Estud.Back.Middlewares;

public class CommandsProcessorTriggerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, EstudDbContext ctx, ISchedulerFactory factory)
    {
        await next(context);

        if (!ctx.HasPendingCommands) return;

        var scheduler = await factory.GetScheduler();
        await scheduler.TriggerCommandsProcessorJob();
    }
}
