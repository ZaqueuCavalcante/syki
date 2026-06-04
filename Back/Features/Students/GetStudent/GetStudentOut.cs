namespace Syki.Back.Features.Students.GetStudent;

public class GetStudentOut : IApiDto<GetStudentOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }
    public int? CurrentCourseOfferingId { get; set; }

    public static IEnumerable<(string, GetStudentOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Maria Souza", Email = "maria@ufal.edu.br", EnrollmentCode = "20251A2B3C4D", Status = StudentStatus.Enrolled, CurrentCourseOfferingId = 1 }),
    ];
}
