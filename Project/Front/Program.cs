var builder = WasmConfigs.CreateHostBuilder(args);

builder.AddMudConfigs();
builder.AddHttpConfigs();
builder.AddAuthConfigs();
builder.AddServicesConfigs();
builder.AddApexChartsConfigs();
builder.AddLocalStorageConfigs();

await builder.Build().RunAsync();
