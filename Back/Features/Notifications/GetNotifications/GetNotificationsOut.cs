using System.Text.Json;

namespace Estud.Back.Features.Notifications.GetNotifications;

public class GetNotificationsOut : IApiDto<GetNotificationsOut>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<GetNotificationsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetNotificationsOut)> GetExamples() =>
    [
        ("Exemplo", new GetNotificationsOut()
        {
            Total = 1,
            Page = 1,
            PageSize = 20,
            Items =
            [
                new GetNotificationsItemOut
                {
                    Id = 1,
                    NotificationType = NotificationType.NewClassActivity,
                    Title = "Nova atividade de classe",
                    Description = "Uma nova atividade foi adicionada à sua turma.",
                    CreatedAt = new DateTime(2026, 1, 1, 12, 0, 0),
                    ViewedAt = null,
                },
            ],
        }),
    ];
}

public class GetNotificationsItemOut
{
    public int Id { get; set; }
    public NotificationType NotificationType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ViewedAt { get; set; }
    public JsonDocument? Metadata { get; set; }
}
