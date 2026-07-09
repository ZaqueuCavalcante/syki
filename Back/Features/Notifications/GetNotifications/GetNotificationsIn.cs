namespace Estud.Back.Features.Notifications.GetNotifications;

public class GetNotificationsIn : IApiDto<GetNotificationsIn>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool UnreadOnly { get; set; }

    public static IEnumerable<(string, GetNotificationsIn)> GetExamples() =>
    [
        ("Exemplo", new GetNotificationsIn() { }),
    ];
}
