var builder = WasmConfigs.CreateHostBuilder(args);

builder.AddMudConfigs();
builder.AddHttpConfigs();
builder.AddAuthConfigs();
builder.AddServicesConfigs();
builder.AddLocalStorageConfigs();

await builder.Build().RunAsync();
