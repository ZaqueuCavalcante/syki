namespace Syki.Shared;

public class StudentOut : IApiDto<StudentOut>
{
    public Guid Id { get; set; }
    public Guid CourseOfferingId { get; set; }
    public string CourseOffering { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string EnrollmentCode { get; set; }

    public static IEnumerable<(string, StudentOut)> GetExamples() =>
    [
        ("Exemplo", new()
{
    Id = Guid.NewGuid(),
    CourseOfferingId = Guid.NewGuid(),
    CourseOffering = "Engenharia de Software - Noturno",
    Name = "Maria Oliveira",
    Email = "maria.oliveira@exemplo.com",
    PhoneNumber = "(88) 99999-1234",
    BirthDate = new DateTime(2000, 5, 20),
    EnrollmentCode = "2025ENG001"
}),
    ];

    public static implicit operator StudentOut(OneOf<StudentOut, ErrorOut> value)
    {
        return value.Success;
    }
}
