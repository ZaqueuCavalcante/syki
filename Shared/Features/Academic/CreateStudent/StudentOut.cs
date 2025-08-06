namespace Syki.Shared;

public class StudentOut
{
    public Guid Id { get; set; }
    public Guid CourseOfferingId { get; set; }
    public string CourseOffering { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string EnrollmentCode { get; set; }

    public static IEnumerable<(string, StudentOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];

    public static implicit operator StudentOut(OneOf<StudentOut, ErrorOut> value)
    {
        return value.Success;
    }
}
