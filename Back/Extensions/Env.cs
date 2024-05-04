namespace Syki.Back.Extensions;

public static class Env
{
    private static string Testing = nameof(Testing);
    private static string Development = nameof(Development);

    public static void SetAsTesting()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Testing);
    }

    public static bool IsTesting()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Testing;
    }

    public static string Get()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
    }

    public static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Development;
    }
}
