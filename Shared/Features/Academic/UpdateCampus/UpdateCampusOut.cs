namespace Syki.Shared;

public class UpdateCampusOut : IApiDto<UpdateCampusOut>
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

    public static IEnumerable<(string, UpdateCampusOut)> GetExamples() =>
    [
        ("Agreste",
        new UpdateCampusOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Agreste",
            State = BrazilState.PE,
            City = "Caruaru",
            Capacity = 220,
        }),
        ("Suassuna",
        new UpdateCampusOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Suassuna",
            State = BrazilState.PE,
            City = "Recife",
            Capacity = 600,
        }),
    ];

    public static implicit operator UpdateCampusOut(OneOf<UpdateCampusOut, ErrorOut> value)
    {
        return value.Success;
    }
}
