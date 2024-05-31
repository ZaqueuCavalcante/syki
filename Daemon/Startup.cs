using Hangfire;
using Syki.Daemon.Tasks;
using Syki.Daemon.Configs;
using Hangfire.PostgreSql;

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
            x.UsePostgreSqlStorage(x => x.UseNpgsqlConnection(configuration.Database().ConnectionString));
        });

        services.AddHangfireServer(x =>
        {
            x.HeartbeatInterval = TimeSpan.FromMinutes(30);
            x.SchedulePollingInterval = TimeSpan.FromMinutes(30);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        RecurringJob.AddOrUpdate<SykiTasksProcessor>("tasks-processor", x => x.Run(), configuration.Tasks().DelayCron());

        app.UseStaticFiles();

        app.UseHangfireDashboard(
            pathMatch: "",
            options: new DashboardOptions()
            {
                FaviconPath = "/favicon.ico",
                Authorization = [ new HangfireAuthFilter(configuration.Hangfire().User, configuration.Hangfire().Password) ]
            }
        );
    }
}
