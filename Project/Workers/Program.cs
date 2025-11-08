
using Exato.Back.Configs;
using Exato.Workers.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuditConfigs();
builder.AddCacheConfigs();
builder.AddEfCoreConfigs();
builder.AddDapperConfigs();
builder.AddSettingsConfigs();
builder.AddServicesConfigs();
builder.AddIdentityConfigs();

builder.AddWorkersHttpConfigs();
builder.AddWorkersQuartzConfigs();
builder.AddWorkersServicesConfigs();
builder.AddWorkersOpenTelemetryConfigs();
builder.AddWorkersHostedServicesConfigs();
builder.AddWorkersMessageHandlersConfigs();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { Status = "Healthy" }));
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.Run();

public partial class Program { }
