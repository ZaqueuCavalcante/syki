namespace Syki.Settings;

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
    public int ExpirationTimeInMinutes { get; set; }

    public AuthSettings(IConfiguration configuration)
    {
        configuration.GetSection("Auth").Bind(this);
    }
}
