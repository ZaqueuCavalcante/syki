namespace Syki.Back.Features.Notifications.MarkNotificationsAsViewed;

public class MarkNotificationsAsViewedIn : IApiDto<MarkNotificationsAsViewedIn>
{
    public bool MarkAll { get; set; }
    public int? NotificationId { get; set; }

    public static IEnumerable<(string, MarkNotificationsAsViewedIn)> GetExamples() =>
    [
        ("Exemplo", new MarkNotificationsAsViewedIn()
        {
            NotificationId = 1,
        }),
    ];
}
