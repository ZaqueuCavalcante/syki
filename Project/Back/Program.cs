var builder = WebApplication.CreateBuilder(args);

builder.AddSettingsConfigs();
builder.AddServicesConfigs();

builder.AddIdentityConfigs();
builder.AddAuthenticationConfigs();
builder.AddAuthorizationConfigs();

builder.AddAuditConfigs();
builder.AddHttpConfigs();

builder.AddCacheConfigs();
builder.AddDapperConfigs();
builder.AddEfCoreConfigs();

builder.AddDocsConfigs();
builder.AddCorsConfigs();

builder.AddOpenTelemetryConfigs();

builder.AddHostConfigs();

var app = builder.Build();

app.UseHttpsConfigs();

app.UseLogs();

app.UseApiPrefix();
app.UseRouting();

app.UseExceptions();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.UseEnrichDbContext();

app.UseDocs();

app.UseControllers();

app.Run();

public partial class Program { }
