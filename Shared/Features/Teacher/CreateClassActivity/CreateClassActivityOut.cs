namespace Syki.Shared;

public class CreateClassActivityOut
{
    public Guid Id { get; set; }

    public static IEnumerable<(string, CreateClassActivityOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = Guid.CreateVersion7() }),
    ];

    public static implicit operator CreateClassActivityOut(OneOf<CreateClassActivityOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
