namespace Syki.Shared;

public class CreateClassActivityIn
{
    /// <summary>
    /// Id da Aula
    /// </summary>
    public Guid LessonId { get; set; }

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
    public DateTime? DueDate { get; set; }
}
