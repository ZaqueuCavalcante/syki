namespace Estud.Back.Features.Parents.CreateParent;

public class CreateParentIn : IApiDto<CreateParentIn>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<CreateParentStudentIn> Students { get; set; } = [];

    public static IEnumerable<(string, CreateParentIn)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Name = "Ana Souza",
            Email = "ana.souza@gmail.com",
            PhoneNumber = "82988887777",
            Students = [new() { StudentId = 1, Relationship = ParentRelationship.Mother }],
        }),
    ];
}

public class CreateParentStudentIn
{
    public int StudentId { get; set; }
    public ParentRelationship Relationship { get; set; }
}
