namespace Estud.Back.Features.Teachers.GetTeacherClassActivity;

public class GetTeacherClassActivityOut : IApiDto<GetTeacherClassActivityOut>
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

    /// <summary>
    /// Total de entregas feitas pelos alunos
    /// </summary>
    public int DeliveredWorks { get; set; }

    /// <summary>
    /// Total de entregas esperadas na atividade
    /// </summary>
    public int TotalWorks { get; set; }

    /// <summary>
    /// Entregas dos alunos matriculados na turma
    /// </summary>
    public List<GetTeacherClassActivityWorkOut> Works { get; set; } = [];

    public static IEnumerable<(string, GetTeacherClassActivityOut)> GetExamples() =>
    [
        ("Exemplo", new GetTeacherClassActivityOut
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
            DeliveredWorks = 1,
            TotalWorks = 2,
            Works =
            [
                new()
                {
                    Id = 1,
                    StudentId = 1,
                    Student = "Maria Souza",
                    Link = "https://github.com/ZaqueuCavalcante/estud",
                    Status = ClassActivityWorkStatus.Delivered,
                    Value = 0,
                },
                new()
                {
                    Id = 2,
                    StudentId = 2,
                    Student = "Chico Ferreira",
                    Link = null,
                    Status = ClassActivityWorkStatus.Pending,
                    Value = 0,
                },
            ],
        }),
    ];
}

public class GetTeacherClassActivityWorkOut
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string Student { get; set; }
    public string? Link { get; set; }
    public ClassActivityWorkStatus Status { get; set; }

    /// <summary>
    /// Nota do aluno na atividade
    /// </summary>
    public decimal Value { get; set; }
}
