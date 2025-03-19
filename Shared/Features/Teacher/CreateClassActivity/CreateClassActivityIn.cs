namespace Syki.Shared;

public class CreateClassActivityIn
{
    /// <summary>
    /// Nota
    /// </summary>
    public StudentClassNoteType Note { get; set; }

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
}
