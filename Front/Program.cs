using Front;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5160") };
httpClient.DefaultRequestHeaders.Add(
    "Authorization",
    "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJmYTZhYWMxNy0zOWUwLTQyNjktOGVmZC1lMmNjNWExZTU5ZjkiLCJzdWIiOiI2YWVlOWYzOS1mYTMzLTQxMjItYjg5ZC1lYWExM2RhNzQzOWUiLCJlbWFpbCI6ImFjYWRlbWljb0Bub3Zhcm9tYS5jb20iLCJmYWN1bGRhZGUiOiI4ZDA4ZTQzNy04YjE4LTRhMTUtYTIzMS00YTIyNjBlNjA0MzIiLCJyb2xlIjoiQWNhZGVtaWNvIiwibmJmIjoxNjk2Mjc2NDc3LCJleHAiOjE2OTk4NzY0NzcsImlhdCI6MTY5NjI3NjQ3NywiaXNzIjoic3lraS1hcGktZGV2ZWxvcG1lbnQiLCJhdWQiOiJzeWtpLWFwaS1kZXZlbG9wbWVudCJ9.VnfqJoHSGtJDZFD9FZafyOGeLHTxAZfgLjlxj_INFxM"
);

builder.Services.AddScoped(sp => httpClient);

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
