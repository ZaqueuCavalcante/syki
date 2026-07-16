namespace Estud.Back.Features.Campi.GetCampi;

public class GetCampiOut : IApiDto<GetCampiOut>
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
                    Name = "Agreste",
                    State = BrazilState.PE,
                    City = "Caruaru",
                    Students = 120,
                },
                new GetCampiItemOut
                {
                    Name = "Suassuna",
                    State = BrazilState.PE,
                    City = "Recife",
                    Students = 234,
                },
            ],
        }),
    ];
}

public class GetCampiItemOut
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

    /// <summary>
    /// Total de alunos
    /// </summary>
    public int Students { get; set; }

    /// <summary>
    /// Total de professores
    /// </summary>
    public int Teachers { get; set; }
}
