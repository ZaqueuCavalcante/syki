namespace Syki.Back.Settings;

public class FrontendSettings
{
    /// <summary>
    /// Base URL of the frontend application (e.g., http://localhost:3000).
    /// Used for redirects after SSO login and other cross-origin flows.
    /// </summary>
    public string Url { get; set; }

    public FrontendSettings(IConfiguration configuration)
    {
        configuration.GetSection("Frontend").Bind(this);

        if (Url.IsEmpty()) throw new InvalidOperationException("Frontend:Url is required.");

        Url = Url.TrimEnd('/');
    }

    /// <summary>
    /// Builds a full URL for a frontend path.
    /// </summary>
    public string BuildUrl(string path = "/")
    {
        if (path.IsEmpty() || path == "/") return Url;

        return $"{Url}{(path.StartsWith('/') ? path : $"/{path}")}";
    }
}

public static class FrontendSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public FrontendSettings Frontend => new(configuration);
    }
}
