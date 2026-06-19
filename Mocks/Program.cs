using Syki.Mocks.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.AddHttpConfigs();
builder.AddEfCoreConfigs();

var app = builder.Build();

app.UseRouting();

app.UseControllers();

app.MapGet("/", () => Results.Ok(new { Status = "Healthy" }));
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

app.Run();

public partial class Program { }
