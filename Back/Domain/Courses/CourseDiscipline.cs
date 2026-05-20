namespace Syki.Back.Domain.Courses;

/// <summary>
/// Vínculo entre curso e disciplina
/// </summary>
public class CourseDiscipline
{
    public int CourseId { get; set; }
    public int DisciplineId { get; set; }
}
