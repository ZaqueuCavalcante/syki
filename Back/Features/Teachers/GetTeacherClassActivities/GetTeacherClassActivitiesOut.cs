namespace Estud.Back.Features.Teachers.GetTeacherClassActivities;

public class GetTeacherClassActivitiesOut : IApiDto<GetTeacherClassActivitiesOut>
{
    public List<GetTeacherClassActivitiesItemOut> Activities { get; set; } = [];

    public static IEnumerable<(string, GetTeacherClassActivitiesOut)> GetExamples() =>
    [
        ("Exemplo", new GetTeacherClassActivitiesOut
        {
            Activities =
            [
                new()
                {
                    Id = 1,
                    ClassId = 1,
                    Note = ClassNoteType.N1,
                    Title = "Trabalho de Grafos",
                    Description = "Implementar o algoritmo de Dijkstra.",
                    Type = ClassActivityType.Work,
                    Status = ClassActivityStatus.Published,
                    Weight = 25,
                    CreatedAt = new DateTime(2026, 3, 10, 14, 0, 0, DateTimeKind.Utc),
                    DueDate = new DateOnly(2026, 3, 20),
                    DueHour = Hour.H22_00,
                    DeliveredWorks = 12,
                    TotalWorks = 40,
                },
            ],
        }),
    ];
}

public class GetTeacherClassActivitiesItemOut
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public ClassNoteType Note { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ClassActivityType Type { get; set; }
    public ClassActivityStatus Status { get; set; }
    public int Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly DueDate { get; set; }
    public Hour DueHour { get; set; }
    public int DeliveredWorks { get; set; }
    public int TotalWorks { get; set; }
}
