namespace Syki.Back.Features.Students.GetStudents;

public class GetStudentsOut : IApiDto<GetStudentsOut>
{
    public int Total { get; set; }
    public List<GetStudentsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetStudentsOut)> GetExamples() =>
    [
        ("Exemplo", new() { Total = 1, Items = [new() { Id = 1, Name = "Maria Souza", Email = "maria@ufal.edu.br", EnrollmentCode = "20251A2B3C4D", Status = StudentStatus.Enrolled }] }),
    ];
}

public class GetStudentsItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string EnrollmentCode { get; set; }
    public StudentStatus Status { get; set; }
    public string Course { get; set; }
}
