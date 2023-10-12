using Front;
using MudBlazor;
using Syki.Front.Auth;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalStorageServices();
builder.Services.AddScoped<SykiDelegatingHandler>();

builder.Services
    //.AddHttpClient("HttpClient", x => x.BaseAddress = new Uri("http://localhost:5160"))
    .AddHttpClient("HttpClient", x => x.BaseAddress = new Uri("https://syki-dev-api.azurewebsites.net"))
    .AddHttpMessageHandler<SykiDelegatingHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("HttpClient"));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SykiAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());

builder.Services.AddAuthorizationCore();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 2_000;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

await builder.Build().RunAsync();
