using Microsoft.Extensions.Configuration;

namespace Syki.Daemon.Settings;

public class TasksSettings
{
    public int Delay { get; set; }

    public TasksSettings(IConfiguration configuration)
    {
        configuration.GetSection("Tasks").Bind(this);
    }
}
