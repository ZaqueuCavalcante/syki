namespace Syki.Shared;

public class UpdateCampusIn
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public BrazilState? State { get; set; }
    public string City { get; set; }
    public int Capacity { get; set; }

    public static IEnumerable<(string, UpdateCampusIn)> GetExamples() =>
    [
        ("Agreste",
        new UpdateCampusIn
        {
            Id = Guid.CreateVersion7(),
            Name = "Agreste",
            State = BrazilState.PE,
            City = "Caruaru",
            Capacity = 300,
        }),
        ("Suassuna",
        new UpdateCampusIn
        {
            Id = Guid.CreateVersion7(),
            Name = "Suassuna",
            State = BrazilState.PE,
            City = "Recife",
            Capacity = 820,
        }),
    ];
}
