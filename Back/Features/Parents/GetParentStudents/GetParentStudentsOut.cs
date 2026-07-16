namespace Estud.Back.Features.Parents.GetParentStudents;

public class GetParentStudentsOut : IApiDto<GetParentStudentsOut>
{
    public List<GetParentStudentsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetParentStudentsOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Items = [new() { Id = 1, Name = "Maria Souza", EnrollmentCode = "20251A2B3C4D", Relationship = ParentRelationship.Mother }],
        }),
    ];
}

public class GetParentStudentsItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string EnrollmentCode { get; set; }
    public ParentRelationship Relationship { get; set; }
}
