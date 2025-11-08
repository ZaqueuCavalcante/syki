namespace Syki.Shared;

public class CreateCampusOut : IApiDto<CreateCampusOut>
{
    public Guid Id { get; set; }

    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    public BrazilState State { get; set; }

    /// <summary>
    /// Cidade
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Capacidade total de alunos
    /// </summary>
    public int Capacity { get; set; }

    public static IEnumerable<(string, CreateCampusOut)> GetExamples() =>
    [
        ("Agreste",
        new CreateCampusOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Agreste",
            State = BrazilState.PE,
            City = "Caruaru",
            Capacity = 150,
        }),
        ("Suassuna",
        new CreateCampusOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Suassuna",
            State = BrazilState.PE,
            City = "Recife",
            Capacity = 500,
        }),
    ];

    public static implicit operator CreateCampusOut(OneOf<CreateCampusOut, ErrorOut> value)
    {
        return value.Success;
    }
}
