namespace Syki.Daemon;

public static class Extensions
{
    public static string DbCnnString(this IConfiguration configuration)
    {
        return configuration.GetSection("Database:ConnectionString").Value ?? "";
    }

    public static string Delay(this IConfiguration configuration)
    {
        var delay = configuration.GetSection("Tasks:Delay").Value;
        return $"*/{delay} * * * * *";
    }
}
