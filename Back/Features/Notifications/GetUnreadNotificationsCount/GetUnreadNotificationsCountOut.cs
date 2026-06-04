namespace Syki.Back.Features.Notifications.GetUnreadNotificationsCount;

public class GetUnreadNotificationsCountOut : IApiDto<GetUnreadNotificationsCountOut>
{
    public int Count { get; set; }

    public static IEnumerable<(string, GetUnreadNotificationsCountOut)> GetExamples() =>
    [
        ("Exemplo", new GetUnreadNotificationsCountOut()
        {
            Count = 3,
        }),
    ];
}
