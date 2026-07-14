namespace Estud.Back.Features.Students.CreateClassActivityWork;

public class CreateClassActivityWorkOut : IApiDto<CreateClassActivityWorkOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateClassActivityWorkOut)> GetExamples() =>
    [
        ("Exemplo", new CreateClassActivityWorkOut { Id = 1 }),
    ];
}
