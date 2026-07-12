namespace Estud.Back.Features.Notifications.GetInstitutionNotification;

public class GetInstitutionNotificationOut : IApiDto<GetInstitutionNotificationOut>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Recipients { get; set; }
    public int Viewed { get; set; }
    public decimal ViewRate { get; set; }
    public List<GetInstitutionNotificationViewsByDayOut> ViewsByDay { get; set; } = [];

    public static IEnumerable<(string, GetInstitutionNotificationOut)> GetExamples() =>
    [
        ("Exemplo", new GetInstitutionNotificationOut()
        {
            Id = 1,
            Title = "Recesso acadêmico",
            Description = "Não haverá aulas na próxima semana.",
            CreatedAt = new DateTime(2026, 1, 1, 12, 0, 0),
            Recipients = 100,
            Viewed = 75,
            ViewRate = 75M,
            ViewsByDay =
            [
                new GetInstitutionNotificationViewsByDayOut { Day = new DateOnly(2026, 1, 1), Views = 50 },
                new GetInstitutionNotificationViewsByDayOut { Day = new DateOnly(2026, 1, 2), Views = 25 },
            ],
        }),
    ];
}

public class GetInstitutionNotificationViewsByDayOut
{
    public DateOnly Day { get; set; }
    public int Views { get; set; }
}
