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

        services.AddHostedService<PostgresListener>();

        services.AddHangfire(x =>
        {
            x.UseMemoryStorage();
            x.UseRecommendedSerializerSettings();
            x.UseSimpleAssemblyNameTypeSerializer();
        });

        services.AddHangfireServer(x =>
        {
            x.ServerName = "Daemon";
            x.SchedulePollingInterval = TimeSpan.FromSeconds(1);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        BackgroundJob.Enqueue<SykiTasksProcessor>(x => x.Run());

        app.UseRouting();
        app.UseStaticFiles();

        app.UseEndpoints(x =>
        {
            x.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));
        });

        app.UseHangfireDashboard(
            pathMatch: "",
            options: new DashboardOptions
            {
                FaviconPath = "/favicon.ico",
                Authorization = [ new HangfireAuthFilter(configuration.Hangfire().User, configuration.Hangfire().Password) ]
            }
        );
    }
}
