using Syki.Daemon.Events;
using Syki.Daemon.Configs;
using Syki.Daemon.Commands;

var builder = WebApplication.CreateBuilder(args);
Audit.Core.Configuration.AuditDisabled = builder.Configuration.Audit().Disabled;

builder.AddServicesConfigs();
builder.AddHandlersConfigs();

builder.AddDapperConfigs();
builder.AddCacheConfigs();

builder.AddQuartzConfigs();

builder.Services.AddHostedService<CommandsProcessorDbListener>();
builder.Services.AddHostedService<DomainEventsProcessorDbListener>();

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.Run();

public partial class Program { }
