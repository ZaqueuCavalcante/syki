namespace Estud.Back.Features.Notifications.CreateNotification;

public class CreateNotificationIn : IApiDto<CreateNotificationIn>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public UsersGroup TargetUsers { get; set; }

    public CreateNotificationIn() {}

    public CreateNotificationIn(
        string title,
        string description,
        UsersGroup targetUsers
    ) {
        Title = title;
        Description = description;
        TargetUsers = targetUsers;
    }

    public static IEnumerable<(string, CreateNotificationIn)> GetExamples() =>
    [
        ("Exemplo", new("Aviso importante", "Descrição do aviso importante.", UsersGroup.All)),
    ];
}
