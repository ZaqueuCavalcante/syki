namespace Syki.Shared;

public class NotificationOut : IApiDto<NotificationOut>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public UsersGroup Target { get; set; }
    public bool Timeless { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Views { get; set; }

    public static IEnumerable<(string, NotificationOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
