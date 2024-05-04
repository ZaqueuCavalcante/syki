using Syki.Daemon.Configs;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.AddAppConfigs();
builder.AddServicesConfigs();

using var host = builder.Build();

await host.RunAsync();
