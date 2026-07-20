namespace Estud.Back.Features.Students.CreateStudent;

public class CreateStudentOut : IApiDto<CreateStudentOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateStudentOut)> GetExamples() =>
    [
        ("Maria", new CreateStudentOut { Id = 1 })
    ];
}
