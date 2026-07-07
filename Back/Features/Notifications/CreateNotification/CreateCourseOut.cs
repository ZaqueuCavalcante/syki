namespace Syki.Back.Features.Notifications.CreateNotification;

public class CreateNotificationOut : IApiDto<CreateNotificationOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateNotificationOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
