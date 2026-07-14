namespace Estud.Back.Features.Students.GetStudentClassActivity;

public class GetStudentClassActivityOut : IApiDto<GetStudentClassActivityOut>
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
    /// Status da entrega do aluno logado
    /// </summary>
    public ClassActivityWorkStatus WorkStatus { get; set; }

    /// <summary>
    /// Link entregue pelo aluno logado
    /// </summary>
    public string? WorkLink { get; set; }

    /// <summary>
    /// Nota do aluno logado na atividade
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Nota do aluno logado ponderada pelo peso da atividade
    /// </summary>
    public decimal PonderedValue { get; set; }

    public static IEnumerable<(string, GetStudentClassActivityOut)> GetExamples() =>
    [
        ("Exemplo", new GetStudentClassActivityOut
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
            WorkStatus = ClassActivityWorkStatus.Delivered,
            WorkLink = "https://github.com/ZaqueuCavalcante/estud",
            Value = 8.5M,
            PonderedValue = 2.125M,
        }),
    ];
}
