namespace Syki.Shared;

public class CreateClassActivityIn : IApiDto<CreateClassActivityIn>
{
    /// <summary>
    /// Nota
    /// </summary>
    public ClassNoteType Note { get; set; }

    /// <summary>
    /// Título
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Descrição
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Tipo
    /// </summary>
    public ClassActivityType Type { get; set; }

    /// <summary>
    /// Peso no intervalo: 0 ≤ Weight ≤ 100
    /// </summary>
    public int Weight { get; set; }

    /// <summary>
    /// Data limite para entrega
    /// </summary>
    public DateOnly DueDate { get; set; }

    /// <summary>
    /// Hora limite para entrega
    /// </summary>
    public Hour DueHour { get; set; }

    public static IEnumerable<(string, CreateClassActivityIn)> GetExamples() =>
    [
        ("Atividade",
        new CreateClassActivityIn
        {
            Title = "Modelagem de Banco de Dados",
            Description = "Modele um banco de dados para um sistema de gerenciamento de biblioteca.",
            DueDate = DateTime.UtcNow.AddDays(7).ToDateOnly(),
            DueHour = Hour.H19_00,
        })
    ];
}
