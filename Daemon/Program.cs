using Syki.Back.Configs;
using Syki.Daemon.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.AddEfCoreConfigs();
builder.AddDapperConfigs();
builder.AddCacheConfigs();
builder.AddSettingsConfigs();
builder.AddServicesConfigs();

builder.AddDaemonAuditConfigs();
builder.AddDaemonSettingsConfigs();
builder.AddDaemonServicesConfigs();
builder.AddDaemonHandlersConfigs();
builder.AddDaemonQuartzConfigs();
builder.AddDaemonOpenTelemetryConfigs();
builder.AddDaemonHostedServicesConfigs();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.Run();

public partial class Program { }
