using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Back.Features.Student.CreateClassActivityWork;

/// <summary>
/// Entrega de uma atividade feita por um aluno
/// </summary>
public class ClassActivityWork : Entity
{
    public Guid Id { get; set; }
    public Guid ClassActivityId { get; set; }
    public Guid SykiStudentId { get; set; }
    public SykiStudent SykiStudent { get; set; }
    public string? Link { get; set; }
    public decimal Note { get; set; }
    public ClassActivityWorkStatus Status { get; set; }

    private ClassActivityWork() { }

    public ClassActivityWork(
        Guid classActivityId,
        Guid studentId
    ) {
        Id = Guid.NewGuid();
        ClassActivityId = classActivityId;
        SykiStudentId = studentId;
        Note = 0;
        Status = ClassActivityWorkStatus.Pending;
    }

    public void AddLink(string link)
    {
        Link = link;
    }

    public OneOf<SykiSuccess, SykiError> AddNote(decimal note)
    {
        if (note < 0 || note > 10) return new InvalidStudentClassNote();

        Note = note;

        AddDomainEvent(new StudentClassNoteAddedDomainEvent(SykiStudentId, ClassActivityId));

        return new SykiSuccess();
    }

    public ClassActivityWorkOut ToOut()
    {
        return new()
        {
            Id = Id,
            StudentId = SykiStudentId,
            StudentName = SykiStudent != null ? SykiStudent.Name : "",
            Link = Link,
        };
    }
}
