using Quartz;

namespace Estud.Back.Extensions;

public static class CommandsExtensions
{
    extension(IScheduler scheduler)
    {
        public async Task TriggerCommandsProcessorJob()
        {
            await scheduler.TriggerJob(new JobKey(nameof(CommandsProcessor)));
        }
    }
}
