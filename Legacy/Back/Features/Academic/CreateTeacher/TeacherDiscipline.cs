namespace Syki.Back.Features.Academic.CreateTeacher;

/// <summary>
/// Vínculo entre professor e disciplina
/// </summary>
public class TeacherDiscipline
{
    public Guid SykiTeacherId { get; set; }
    public Guid DisciplineId { get; set; }
}
