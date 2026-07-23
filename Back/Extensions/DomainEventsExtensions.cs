using Quartz;

namespace Estud.Back.Extensions;

public static class DomainEventsExtensions
{
    extension(IScheduler scheduler)
    {
        public async Task TriggerDomainEventsProcessorJob()
        {
            await scheduler.TriggerJob(new JobKey(nameof(DomainEventsProcessor)));
        }
    }
}
