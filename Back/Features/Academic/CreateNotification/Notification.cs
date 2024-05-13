namespace Syki.Back.Features.Academic.CreateNotification;

public class Notification
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<UserNotification> Users { get; set; }

    public string Views { get; set; }

    public Notification(
        Guid institutionId,
        string title,
        string description
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
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
