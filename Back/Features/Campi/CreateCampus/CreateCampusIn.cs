namespace Estud.Back.Features.Campi.CreateCampus;

public class CreateCampusIn : IApiDto<CreateCampusIn>
{
    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    public BrazilState? State { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    public string City { get; set; }

    public static IEnumerable<(string, CreateCampusIn)> GetExamples() =>
    [
        ("Agreste",
        new CreateCampusIn
        {
            Name = "Agreste",
            State = BrazilState.PE,
            City = "Caruaru",
        }),

        ("Suassuna",
        new CreateCampusIn
        {
            Name = "Suassuna",
            State = BrazilState.PE,
            City = "Recife",
        }),
    ];
}
