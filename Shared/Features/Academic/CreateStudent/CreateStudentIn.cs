namespace Syki.Shared;

public class CreateStudentIn
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Guid CourseOfferingId { get; set; }
    public string? PhoneNumber { get; set; }

    public static IEnumerable<(string, CreateStudentIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];

    public static CreateStudentIn Seed(string name, Guid courseOfferingId)
    {
        return new()
        {
            Name = name,
            CourseOfferingId = courseOfferingId,
            Email = $"aluno.{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.seed.com"
        };
    }
}
