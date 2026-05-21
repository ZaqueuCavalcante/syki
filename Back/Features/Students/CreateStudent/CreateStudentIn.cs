namespace Syki.Back.Features.Students.CreateStudent;

public class CreateStudentIn : IApiDto<CreateStudentIn>
{
    public string Name { get; set; }
    public string Email { get; set; }

    public static IEnumerable<(string, CreateStudentIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
