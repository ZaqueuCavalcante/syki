namespace Syki.Shared;

public class CreateClassActivityIn
{
    /// <summary>
    /// Título
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Descrição
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Data limite para entrega
    /// </summary>
    public DateOnly DueDate { get; set; }

    /// <summary>
    /// Hora limite para entrega
    /// </summary>
    public Hour DueHour { get; set; }
}
