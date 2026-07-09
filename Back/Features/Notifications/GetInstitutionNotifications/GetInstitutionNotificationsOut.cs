namespace Estud.Back.Features.Notifications.GetInstitutionNotifications;

public class GetInstitutionNotificationsOut : IApiDto<GetInstitutionNotificationsOut>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<GetInstitutionNotificationsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetInstitutionNotificationsOut)> GetExamples() =>
    [
        ("Exemplo", new GetInstitutionNotificationsOut()
        {
            Total = 1,
            Page = 1,
            PageSize = 20,
            Items =
            [
                new GetInstitutionNotificationsItemOut
                {
                    Id = 1,
                    Title = "Recesso acadêmico",
                    Description = "Não haverá aulas na próxima semana.",
                    CreatedAt = new DateTime(2026, 1, 1, 12, 0, 0),
                    Recipients = 100,
                    Viewed = 75,
                    ViewRate = 75M,
                },
            ],
        }),
    ];
}

public class GetInstitutionNotificationsItemOut
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Recipients { get; set; }
    public int Viewed { get; set; }
    public decimal ViewRate { get; set; }
}
