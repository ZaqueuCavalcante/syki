namespace Syki.Back.Features.Campi.CreateCampus;

public class CreateCampusOut : IApiDto<CreateCampusOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateCampusOut)> GetExamples() =>
    [
        ("Agreste", new CreateCampusOut { Id = 1 })
    ];
}
