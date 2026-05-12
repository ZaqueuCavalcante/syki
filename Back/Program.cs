var builder = WebApplication.CreateBuilder(args);

builder.AddSettingsConfigs();
builder.AddServicesConfigs();
builder.AddQuartzConfigs();
builder.AddCommandConfigs();

builder.AddIdentityConfigs();
builder.AddAuthenticationConfigs();
builder.AddAuthorizationConfigs();

builder.AddHttpConfigs();
builder.AddCacheConfigs();
builder.AddRateLimitingConfigs();

builder.AddAuditConfigs();
builder.AddDapperConfigs();
builder.AddEfCoreConfigs();

builder.AddDocsConfigs();
builder.AddCorsConfigs();

builder.AddHostConfigs();
builder.AddOpenTelemetryConfigs();

var app = builder.Build();

app.UseLogs();
app.UseCors();

app.UseRouting();
app.UseExceptions();

app.UseAuthentication();
app.UseRateLimiting();
app.UseAuthorization();
app.UseEnrichDbContext();

app.UseDocs();

app.UseCommandsProcessorTrigger();

app.UseControllers();

app.Run();

public partial class Program { }
