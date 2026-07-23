var builder = WebApplication.CreateBuilder(args);

builder.AddSettingsConfigs();
builder.AddServicesConfigs();
builder.AddQuartzConfigs();
builder.AddDomainEventConfigs();
builder.AddCommandConfigs();

builder.AddIdentityConfigs();
builder.AddDataProtectionConfigs();
builder.AddAuthenticationConfigs();
builder.AddAuthorizationConfigs();

builder.AddHttpConfigs();
builder.AddCacheConfigs();
builder.AddRateLimitingConfigs();

builder.AddAuditConfigs();
builder.AddDapperConfigs();
builder.AddEntityFrameworkConfigs();

builder.AddDocsConfigs();
builder.AddCorsConfigs();

builder.AddHostConfigs();
builder.AddOpenTelemetryConfigs();

var app = builder.Build();

app.UseForwardedHeaders();
app.UseHttpsConfigs();

app.UseLogs();
app.UseCors();

app.UseApiPrefix();
app.UseRouting();
app.UseExceptions();

app.UseAuthentication();
app.UseRateLimiting();
app.UseAuthorization();
app.UseEnrichDbContext();

app.UseDocs();

app.UseBackgroundProcessorsTrigger();

app.UseControllers();

app.Run();

public partial class Program { }
