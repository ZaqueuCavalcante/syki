namespace Syki.Shared;

public class GetCampiOut
{
    public int Total { get; set; }
    public List<GetCampiItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCampiOut)> GetExamples() =>
    [
        ("Campi",
        new GetCampiOut()
        {
            Total = 2,
            Items =
            [
                new GetCampiItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Agreste",
                    State = BrazilState.PE,
                    City = "Caruaru",
                    Capacity = 150,
                    Students = 120,
                    FillRate = 80,
                },
                new GetCampiItemOut
                {
                    Id = Guid.NewGuid(),
                    Name = "Suassuna",
                    State = BrazilState.PE,
                    City = "Recife",
                    Capacity = 500,
                    Students = 234,
                    FillRate = 46.80M,
                },
            ],
        }),
    ];
}

public class GetCampiItemOut
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
    /// Total de professores
    /// </summary>
    public int Teachers { get; set; }

    /// <summary>
    /// Taxa de ocupação
    /// </summary>
    public decimal FillRate { get; set; }

    public string GetFillRate()
    {
        return $"{FillRate.Format()}%";
    }

    public override string ToString()
    {
        return Name;
    }
}
