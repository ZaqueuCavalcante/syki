using Hangfire;
using Syki.Daemon.Events;
using Syki.Daemon.Configs;
using Syki.Daemon.Startup;
using Syki.Daemon.Commands;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

builder.AddServicesConfigs();
builder.AddHandlersConfigs();

builder.AddDapperConfigs();
builder.AddCacheConfigs();

builder.Services.AddHostedService<EnqueueProcessors>();
builder.Services.AddHostedService<CommandsProcessorDbListener>();
builder.Services.AddHostedService<DomainEventsProcessorDbListener>();

builder.Services.AddHangfire(x =>
{
    x.UseMemoryStorage();
    x.UseRecommendedSerializerSettings();
    x.UseSimpleAssemblyNameTypeSerializer();
});

builder.Services.AddHangfireServer(x =>
{
    x.ServerName = "Daemon";
    x.SchedulePollingInterval = TimeSpan.FromSeconds(60);
});

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.UseHangfireDashboard(
    pathMatch: "",
    options: new DashboardOptions
    {
        FaviconPath = "/favicon.ico",
        Authorization = [ new HangfireAuthFilter(builder.Configuration.Hangfire().User, builder.Configuration.Hangfire().Password) ]
    }
);

app.Run();

public partial class Program { }
