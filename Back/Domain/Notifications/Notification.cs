using System.Text.Json;

namespace Syki.Back.Domain.Notifications;

public class Notification
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public JsonDocument? Metadata { get; set; }

    public Notification() { }

    public Notification(
        int institutionId,
        NotificationType notificationType,
        string title,
        string description,
        object? metadata = null)
    {
        InstitutionId = institutionId;
        NotificationType = notificationType;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        Metadata = metadata != null ? JsonDocument.Parse(metadata.Serialize()) : null;
    }
}
