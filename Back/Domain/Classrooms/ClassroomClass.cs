namespace Estud.Back.Domain.Classrooms;

/// <summary>
/// Vínculo entre Sala de Aula e Turma
/// </summary>
public class ClassroomClass
{
    public int ClassroomId { get; set; }
    public int ClassId { get; set; }
    public bool IsActive { get; set; }

    private ClassroomClass() {}

    public ClassroomClass(
        int classroomId,
        int classId
    ) {
        ClassroomId = classroomId;
        ClassId = classId;
        IsActive = true;
    }
}
