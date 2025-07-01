using Quartz;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Syki.Back.Database.Interceptors;

public sealed class CommandsDelayInterceptor : SaveChangesInterceptor
{
    private List<DateTime> _delays = [];

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
        _delays = eventData.Context.ChangeTracker
            .Entries<Command>()
            .Where(entry => entry.State == EntityState.Added && entry.Entity.NotBefore != null)
            .Select(x => x.Entity.NotBefore!.Value)
            .ToList();

        return await Task.Run(() => result);
	}

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (_delays.Count == 0) return result;

        var schedulerFactory = eventData.Context.GetService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        foreach (var delay in _delays)
        {
            var job = JobBuilder.Create<NotifyNewCommandJob>().Build();
            var trigger = Quartz.TriggerBuilder.Create()
                .StartAt(delay)
                .Build();
            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }

        return result;
    }
}

public class NotifyNewCommandJob(SykiDbContext ctx) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await ctx.Database.ExecuteSqlRawAsync($"NOTIFY new_command", context.CancellationToken);
    }
}
