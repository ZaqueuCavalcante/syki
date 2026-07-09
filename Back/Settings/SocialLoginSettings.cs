using Microsoft.AspNetCore.Authentication.Google;

namespace Estud.Back.Settings;

public class SocialLoginSettings : SettingsBase
{
    public GoogleLoginSettings Google { get; set; } = new();

    public SocialLoginSettings(IConfiguration configuration)
    {
        configuration.GetSection("SocialLogin").Bind(this);

        if (Google.Enabled)
        {
            RequireNonEmpty(Google.ClientId);
            RequireNonEmpty(Google.ClientSecret);
        }
    }
}

public class GoogleLoginSettings
{
    public bool Enabled { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string? TokenEndpoint { get; set; }
    public string? AuthorizationEndpoint { get; set; }
    public string? UserInformationEndpoint { get; set; }
}

public static class SocialLoginSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public SocialLoginSettings SocialLogin => new(configuration);
    }

    extension(GoogleOptions options)
    {
        public void OverrideWith(GoogleLoginSettings settings)
        {
            if (settings.AuthorizationEndpoint.HasValue())
                options.AuthorizationEndpoint = settings.AuthorizationEndpoint!;

            if (settings.TokenEndpoint.HasValue())
                options.TokenEndpoint = settings.TokenEndpoint!;

            if (settings.UserInformationEndpoint.HasValue())
                options.UserInformationEndpoint = settings.UserInformationEndpoint!;
        }
    }
}
