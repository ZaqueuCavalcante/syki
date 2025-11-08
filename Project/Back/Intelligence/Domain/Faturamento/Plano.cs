namespace Exato.Back.Intelligence.Domain.Faturamento;

/// <summary>
/// Planos de faturamento disponíveis para clientes.
/// </summary>
public class Plano
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string? Descricao { get; set; }

    public DateTime DataInclusao { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public DateTime? DataExclusao { get; set; }
}
