namespace Syki.Back.Settings;

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
    public int ExpirationTimeInMinutes { get; set; }

    public string GoogleClientId { get; set; }
    public string GoogleClientSecret { get; set; }

    public AuthSettings(IConfiguration configuration)
    {
        configuration.GetSection("Auth").Bind(this);
    }
}
