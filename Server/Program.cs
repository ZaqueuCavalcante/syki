var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.WebHost
    .UseIISIntegration()
    .UseUrls("http://*:5002")
    .UseSetting(WebHostDefaults.DetailedErrorsKey, "true");

var app = builder.Build();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapFallbackToFile("index.html");

await app.RunAsync();

public partial class Program { }
