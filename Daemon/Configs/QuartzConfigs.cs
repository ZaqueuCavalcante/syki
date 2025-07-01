using Quartz;

namespace Syki.Daemon.Configs;

public static class QuartzConfigs
{
    public static void AddDaemonQuartzConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz();
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
