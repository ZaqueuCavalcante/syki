using Syki.Daemon.Configs;

var builder = Host.CreateDefaultBuilder(args);

builder.AddAppConfigs();
builder.AddServicesConfigs();

using var host = builder.Build();

await host.RunAsync();

public partial class Program { }
