using Microsoft.Extensions.Configuration;

namespace Syki.Daemon.Settings;

public class EmailSettings
{
    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
    public string FrontUrl { get; set; }

    public EmailSettings(IConfiguration configuration)
    {
        configuration.GetSection("Email").Bind(this);
    }
}
