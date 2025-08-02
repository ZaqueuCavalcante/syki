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

    public static implicit operator CampusOut(OneOf<CampusOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
