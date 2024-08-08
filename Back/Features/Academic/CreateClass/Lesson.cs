namespace Syki.Back.Features.Academic.CreateClass;

/// <summary>
/// Representa uma Aula dentro de uma Turma.
/// </summary>
public class Lesson
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
}
