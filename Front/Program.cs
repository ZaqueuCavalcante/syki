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
    "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIzZTc1ZTE0Yi0zNjNkLTRiM2ItYmJjZC0yNTMxNDkxYjA4Y2QiLCJzdWIiOiIzIiwiZW1haWwiOiJhY2FkZW1pY29Abm92YXJvbWEuY29tIiwiZmFjdWxkYWRlIjoiMSIsInJvbGUiOiJBY2FkZW1pY28iLCJuYmYiOjE2OTM5OTgwNzUsImV4cCI6MTY5NzU5ODA3NSwiaWF0IjoxNjkzOTk4MDc1LCJpc3MiOiJzeWtpLWFwaS1kZXZlbG9wbWVudCIsImF1ZCI6InN5a2ktYXBpLWRldmVsb3BtZW50In0.XhUTzGXRvVznklqHE_errGoAS4rmWCqx9auEpN3Li2I"
);

builder.Services.AddScoped(sp => httpClient);

builder.Services.AddMudServices(config => config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter);

await builder.Build().RunAsync();
