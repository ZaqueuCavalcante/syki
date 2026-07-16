namespace Estud.Back.Features.Campi.UpdateCampus;

public class UpdateCampusIn : IApiDto<UpdateCampusIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public BrazilState? State { get; set; }
    public string City { get; set; }

    public static IEnumerable<(string, UpdateCampusIn)> GetExamples() =>
    [
        ("Agreste",
        new UpdateCampusIn
        {
            Name = "Agreste",
            State = BrazilState.PE,
            City = "Caruaru",
        }),
        ("Suassuna",
        new UpdateCampusIn
        {
            Name = "Suassuna",
            State = BrazilState.PE,
            City = "Recife",
        }),
    ];
}
