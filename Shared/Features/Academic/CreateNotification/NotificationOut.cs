namespace Syki.Shared;

public class NotificationOut
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Timeless { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Views { get; set; }
}
