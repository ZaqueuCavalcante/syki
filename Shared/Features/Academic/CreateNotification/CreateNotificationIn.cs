namespace Syki.Shared;

public class CreateNotificationIn : IApiDto<CreateNotificationIn>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public UsersGroup TargetUsers { get; set; }
    public bool Timeless { get; set; }

    public CreateNotificationIn() {}

    public CreateNotificationIn(
        string title,
        string description,
        UsersGroup targetUsers,
        bool timeless
    ) {
        Title = title;
        Description = description;
        TargetUsers = targetUsers;
        Timeless = timeless;
    }

    public static IEnumerable<(string, CreateNotificationIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
