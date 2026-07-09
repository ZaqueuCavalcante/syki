using Estud.Back.Domain.Students;

namespace Estud.Back.Domain.Classes;

/// <summary>
/// Entrega de uma atividade feita por um aluno
/// </summary>
public class ClassActivityWork
{
    public int Id { get; set; }
    public int ClassActivityId { get; set; }
    public int StudentId { get; set; }
    public EstudStudent Student { get; set; }
    public string? Link { get; set; }
    public decimal Note { get; set; }
    public ClassActivityWorkStatus Status { get; set; }

    private ClassActivityWork() { }

    public ClassActivityWork(
        int classActivityId,
        int studentId
    ) {
        ClassActivityId = classActivityId;
        StudentId = studentId;
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
}
