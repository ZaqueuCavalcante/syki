namespace Estud.Back.Features.Teachers.CreateClassActivity;

public class CreateClassActivityOut : IApiDto<CreateClassActivityOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateClassActivityOut)> GetExamples() =>
    [
        ("Exemplo", new CreateClassActivityOut { Id = 1 }),
    ];
}
