namespace Syki.Shared;

public class CrossLoginOut : IApiDto<CrossLoginOut>
{
    public string AccessToken { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }

    public static IEnumerable<(string, CrossLoginOut)> GetExamples() =>
    [
        ("Aluno", new() { Id = Guid.CreateVersion7(), Name = "Carlos Nobre", Email = "carlos.nobre@syki.com", Role = UserRole.Student }),
        ("Professor", new() { Id = Guid.CreateVersion7(), Name = "Josenilton Gomes", Email = "josenilton.gomes@syki.com", Role = UserRole.Teacher }),
    ];

    public static implicit operator CrossLoginOut(OneOf<CrossLoginOut, ErrorOut> value)
    {
        return value.Success;
    }
}
