namespace Syki.Shared;

public class CreateStudentIn
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public Guid CourseOfferingId { get; set; }

    public static IEnumerable<(string, CreateStudentIn)> GetExamples() =>
    [
        ("Exemplo", new()
{
    Name = "Maria Oliveira",
    Email = "maria.oliveira@exemplo.com",
    PhoneNumber = "(88) 99999-1234",
    BirthDate = new DateTime(2000, 5, 20),
    CourseOfferingId = Guid.NewGuid()
}),
    ];

    public static CreateStudentIn Seed(string name, Guid courseOfferingId)
{
    return new()
    {
        Name = name,
        CourseOfferingId = courseOfferingId,
        Email = $"aluno.{Guid.NewGuid().ToString().OnlyNumbers()[..8]}@syki.seed.com",
        PhoneNumber = "(88) 98888-0000",
        BirthDate = DateTime.Today.AddYears(-18)
    };
}
}
