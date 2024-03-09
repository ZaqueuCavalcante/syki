namespace Syki.Back.CreateNotification;

public class Notification
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<UserNotification> Users { get; set; }

    public string Views { get; set; }

    public Notification(Guid faculdadeId, string title, string description)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.Now;
    }

    public NotificationOut ToOut()
    {
        return new NotificationOut
        {
            Id = Id,
            Title = Title,
            Description = Description,
            CreatedAt = CreatedAt,
            Views = Views,
        };
    }
}
