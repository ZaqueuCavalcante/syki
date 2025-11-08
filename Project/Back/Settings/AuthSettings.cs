namespace Exato.Back.Settings;

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
    public bool CookieSecure { get; set; } = true;
    public string AzureClientId { get; set; }
    public string AzureClientSecret { get; set; }
    public string AzureAuthority { get; set; }

    public AuthSettings(IConfiguration configuration)
    {
        configuration.GetSection("Auth").Bind(this);
    }
}

public static class AuthSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public AuthSettings Auth => new(configuration);
    }
}
