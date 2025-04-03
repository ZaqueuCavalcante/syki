var builder = WebApplication.CreateBuilder(args);

builder.AddSettingsConfigs();
builder.AddServicesConfigs();

builder.AddIdentityConfigs();
builder.AddAuthenticationConfigs();
builder.AddAuthorizationConfigs();

builder.AddHttpConfigs();
builder.AddCacheConfigs();
builder.AddRateLimiterConfigs();

builder.AddAuditConfigs();
builder.AddDapperConfigs();
builder.AddEfCoreConfigs();

builder.AddDocsConfigs();
builder.AddCorsConfigs();

builder.AddHostConfigs();
builder.AddOpenTelemetryConfigs();

var app = builder.Build();

// app.Services.CreateScope().ServiceProvider.GetRequiredService<SykiDbContext>().ResetDb(); 

app.UseLogs();
app.UseCors();

app.UseRouting();
app.UseRateLimiter();
app.UseExceptions();
app.UseCustomHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.UseAudit();
app.UseDocs();

app.UseControllers();

app.Run();

public partial class Program { }
