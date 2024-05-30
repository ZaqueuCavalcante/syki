using Hangfire;
using Syki.Daemon.Tasks;
using Syki.Daemon.Configs;
using Hangfire.MemoryStorage;

namespace Syki.Daemon;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServicesConfigs();

        services.AddHangfire(x =>
        {
            x.UseRecommendedSerializerSettings();
            x.UseSimpleAssemblyNameTypeSerializer();
            // x.UsePostgreSqlStorage(x => x.UseNpgsqlConnection(configuration.DbCnnString()));
            x.UseMemoryStorage();
        });

        services.AddHangfireServer(x =>
        {
            x.HeartbeatInterval = TimeSpan.FromSeconds(5);
            x.SchedulePollingInterval = TimeSpan.FromSeconds(5);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        RecurringJob.AddOrUpdate<SykiTasksProcessor>("tasks-processor", x => x.Run(), configuration.Delay());

        app.UseHangfireDashboard(
            pathMatch: "",
            options: new DashboardOptions()
            {
                Authorization = [ new HangfireAuthFilter(configuration.HangfireUser(), configuration.HangfirePassword()) ]
            }
        );
    }
}
