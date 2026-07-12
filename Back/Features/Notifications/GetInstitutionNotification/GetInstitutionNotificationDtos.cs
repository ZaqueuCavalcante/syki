namespace Estud.Back.Features.Notifications.GetInstitutionNotification;

public class NotificationRow
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Recipients { get; set; }
    public int Viewed { get; set; }
}
