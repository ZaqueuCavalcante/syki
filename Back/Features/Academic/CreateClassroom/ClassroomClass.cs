namespace Syki.Back.Features.Academic.CreateClassroom;

/// <summary>
/// Vínculo entre Sala de Aula e Turma
/// </summary>
public class ClassroomClass
{
    public Guid ClassroomId { get; set; }
    public Guid ClassId { get; set; }
    public bool IsActive { get; set; }

    private ClassroomClass() {}

    public ClassroomClass(
        Guid classroomId,
        Guid classId
    ) {
        ClassroomId = classroomId;
        ClassId = classId;
        IsActive = true;
    }
}
