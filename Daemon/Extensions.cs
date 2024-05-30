namespace Syki.Daemon;

public static class Extensions
{
    public static string DbCnnString(this IConfiguration configuration)
    {
        return configuration.GetSection("Database:ConnectionString").Value ?? throw new Exception("Null Database:ConnectionString");
    }

    public static string Delay(this IConfiguration configuration)
    {
        var delay = configuration.GetSection("Tasks:Delay").Value ?? throw new Exception("Null Tasks:Delay");
        return $"*/{delay} * * * * *";
    }

    public static string HangfireUser(this IConfiguration configuration)
    {
        return configuration.GetSection("Hangfire:User").Value ?? throw new Exception("Null Hangfire:User");
    }
    public static string HangfirePassword(this IConfiguration configuration)
    {
        return configuration.GetSection("Hangfire:Password").Value ?? throw new Exception("Null Hangfire:Password");
    }
}
