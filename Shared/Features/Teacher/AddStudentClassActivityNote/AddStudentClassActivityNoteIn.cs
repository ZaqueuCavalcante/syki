namespace Syki.Shared;

public class AddStudentClassActivityNoteIn
{
    public Guid StudentId { get; set; }
    public decimal Value { get; set; }

    public AddStudentClassActivityNoteIn() {}

    public AddStudentClassActivityNoteIn(Guid studentId, decimal value)
    {
        StudentId = studentId;
        Value = value;
    }

    public static IEnumerable<(string, AddStudentClassActivityNoteIn)> GetExamples() =>
    [
        ("Nota do trabalho", new() { StudentId = Guid.CreateVersion7(), Value = 5.48M }),
        ("Nota da prova", new() { StudentId = Guid.CreateVersion7(), Value = 10.00M }),
    ];
}
