namespace Estud.Back.Features.Campi.UpdateCampus;

public class UpdateCampusOut : IApiDto<UpdateCampusOut>
{
    public int Id { get; set; }

    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    public BrazilState State { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    public string City { get; set; }

    public static IEnumerable<(string, UpdateCampusOut)> GetExamples() =>
    [
        ("Agreste",
        new UpdateCampusOut
        {
            Name = "Agreste",
            State = BrazilState.PE,
            City = "Caruaru",
        }),
        ("Suassuna",
        new UpdateCampusOut
        {
            Name = "Suassuna",
            State = BrazilState.PE,
            City = "Recife",
        }),
    ];
}
