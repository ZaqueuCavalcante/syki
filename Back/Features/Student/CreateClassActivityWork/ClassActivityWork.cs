namespace Syki.Back.Features.Student.CreateClassActivityWork;

/// <summary>
/// Entrega de uma atividade feita por um aluno
/// </summary>
public class ClassActivityWork
{
    public Guid Id { get; set; }
    public Guid ClassActivityId { get; set; }
    public Guid SykiStudentId { get; set; }
    public string Link { get; set; }

    private ClassActivityWork() { }

    public ClassActivityWork(
        Guid classActivityId,
        Guid studentId,
        string link
    ) {
        Id = Guid.NewGuid();
        ClassActivityId = classActivityId;
        SykiStudentId = studentId;
        Link = link;
    }

    public CreateClassActivityWorkOut ToOut()
    {
        return new()
        {
            Id = Id,
        };
    }
}
