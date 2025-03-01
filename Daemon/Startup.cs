using Hangfire;
using Syki.Daemon.Commands;
using Syki.Daemon.Events;
using Syki.Daemon.Configs;
using Hangfire.MemoryStorage;

namespace Syki.Daemon;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServicesConfigs();
        services.AddHandlersConfigs();

        services.AddDapperConfigs();
        services.AddCacheConfigs();

        services.AddHostedService<CommandsProcessorDbListener>();
        services.AddHostedService<DomainEventsProcessorDbListener>();

        services.AddHangfire(x =>
        {
            x.UseMemoryStorage();
            x.UseRecommendedSerializerSettings();
            x.UseSimpleAssemblyNameTypeSerializer();
        });

        services.AddHangfireServer(x =>
        {
            x.ServerName = "Daemon";
            x.SchedulePollingInterval = TimeSpan.FromSeconds(60);
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        BackgroundJob.Enqueue<CommandsProcessor>(x => x.Run());
        BackgroundJob.Enqueue<DomainEventsProcessor>(x => x.Run());

        RecurringJob.AddOrUpdate<CommandsProcessedCleaner>(nameof(CommandsProcessedCleaner), x => x.Run(), Cron.Daily(23));

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
