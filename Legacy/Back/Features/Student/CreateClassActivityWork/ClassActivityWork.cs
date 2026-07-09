using Estud.Back.Features.Academic.CreateStudent;

namespace Estud.Back.Features.Student.CreateClassActivityWork;

/// <summary>
/// Entrega de uma atividade feita por um aluno
/// </summary>
public class ClassActivityWork
{
    public Guid Id { get; set; }
    public Guid ClassActivityId { get; set; }
    public Guid EstudStudentId { get; set; }
    public EstudStudent EstudStudent { get; set; }
    public string? Link { get; set; }
    public decimal Note { get; set; }
    public ClassActivityWorkStatus Status { get; set; }

    private ClassActivityWork() { }

    public ClassActivityWork(
        Guid classActivityId,
        Guid studentId
    ) {
        Id = Guid.CreateVersion7();
        ClassActivityId = classActivityId;
        EstudStudentId = studentId;
        Note = 0;
        Status = ClassActivityWorkStatus.Pending;
    }

    public void AddLink(string link)
    {
        Link = link;
        Status = ClassActivityWorkStatus.Delivered;
    }

    public OneOf<EstudSuccess, EstudError> AddNote(decimal note)
    {
        if (note < 0 || note > 10) return new InvalidStudentClassNote();

        Note = note;
        Status = ClassActivityWorkStatus.Finalized;

        return new EstudSuccess();
    }

    public ClassActivityWorkOut ToOut()
    {
        return new()
        {
            Id = Id,
            StudentId = EstudStudentId,
            StudentName = EstudStudent != null ? EstudStudent.Name : "",
            Status = Status,
            Note = Note,
            Link = Link,
        };
    }
}
