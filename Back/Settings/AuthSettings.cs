namespace Syki.Back.Settings;

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
    public bool CookieSecure { get; set; }
    public string? CookieDomain { get; set; }

    public AuthSettings(IConfiguration configuration)
    {
        configuration.GetSection("Auth").Bind(this);
    }
}

public static class AuthSettingsExtensions
{
    public static AuthSettings Auth(this IConfiguration configuration) => new(configuration);
}
