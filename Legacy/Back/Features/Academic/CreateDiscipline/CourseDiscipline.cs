namespace Syki.Back.Features.Academic.CreateDiscipline;

/// <summary>
/// Vínculo entre curso e disciplina
/// </summary>
public class CourseDiscipline
{
    public Guid CourseId { get; set; }
    public Guid DisciplineId { get; set; }
}
