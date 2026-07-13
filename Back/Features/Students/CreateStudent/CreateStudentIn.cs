namespace Estud.Back.Features.Students.CreateStudent;

public class CreateStudentIn : IApiDto<CreateStudentIn>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? Birthdate { get; set; }

    public static IEnumerable<(string, CreateStudentIn)> GetExamples() =>
    [
        ("Exemplo", new() { Name = "Maria Souza", Email = "maria@ufal.edu.br", PhoneNumber = "82988887777", Birthdate = new DateOnly(2000, 5, 10) }),
    ];
}
