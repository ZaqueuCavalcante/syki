using Audit.Core;

namespace Syki.Daemon;

public static class Program
{
    public static void Main(string[] args)
    {
        Configuration.AuditDisabled = true;
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>()
            );
}
