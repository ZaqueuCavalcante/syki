namespace Syki.Back.Features.Classes.CreateClass;

public class CreateClassOut : IApiDto<CreateClassOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateClassOut)> GetExamples() =>
    [
        ("Banco de Dados", new CreateClassOut { Id = 1 }),
    ];
}
