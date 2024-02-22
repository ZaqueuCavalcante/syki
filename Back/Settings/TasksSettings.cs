namespace Syki.Back.Settings;

public class TasksSettings
{
    public int Delay { get; set; }

    public TasksSettings(IConfiguration configuration)
    {
        configuration.GetSection("Tasks").Bind(this);
    }
}
