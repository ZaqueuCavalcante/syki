using Quartz;

namespace Exato.Workers.Configs;

public static class QuartzConfigs
{
    public static void AddWorkersQuartzConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz();
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
