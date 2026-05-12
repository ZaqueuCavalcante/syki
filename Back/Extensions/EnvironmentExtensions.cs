namespace Syki.Back.Extensions;

public static class EnvironmentExtensions
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

    public static bool IsDevelopment()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Development;
    }

    public static bool IsDevelopmentOrTesting()
    {
        return IsDevelopment() || IsTesting();
    }

    public static string DeployHash = Guid.NewGuid().ToString()[^8..];
}
