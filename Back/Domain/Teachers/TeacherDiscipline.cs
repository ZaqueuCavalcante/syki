namespace Estud.Back.Domain.Teachers;

/// <summary>
/// Vínculo entre professor e disciplina
/// </summary>
public class TeacherDiscipline
{
    public int TeacherId { get; set; }
    public int DisciplineId { get; set; }
}
