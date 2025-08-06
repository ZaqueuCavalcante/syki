namespace Syki.Shared;

public class CampusOut
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

    /// <summary>
    /// Total de alunos
    /// </summary>
    public int Students { get; set; }

    /// <summary>
    /// Taxa de ocupação
    /// </summary>
    public decimal FillRate { get; set; }

    public static IEnumerable<(string, CampusOut)> GetExamples() =>
    [
        ("Agreste",
        new CampusOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Agreste",
            State = BrazilState.PE,
            City = "Caruaru",
            Capacity = 150,
            Students = 120,
            FillRate = 80,
        }),
        ("Suassuna",
        new CampusOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Suassuna",
            State = BrazilState.PE,
            City = "Recife",
            Capacity = 500,
            Students = 234,
            FillRate = 46.80M,
        }),
    ];

    public static implicit operator CampusOut(OneOf<CampusOut, ErrorOut> value)
    {
        return value.Success;
    }
}
