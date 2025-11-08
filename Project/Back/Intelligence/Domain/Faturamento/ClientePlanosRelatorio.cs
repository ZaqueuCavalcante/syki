namespace Exato.Back.Intelligence.Domain.Faturamento;

/// <summary>
/// Atribuições de planos aos clientes.
/// </summary>
public class ClientePlanosRelatorio
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int PlanoId { get; set; }

    /// <summary>
    /// Indica se a atribuição está ativa.
    /// </summary>
    public bool? Ativo { get; set; }

    public DateTime DataAtribuicao { get; set; }

    /// <summary>
    /// Quando o plano foi desativado para o cliente (NULL = ativo).
    /// </summary>
    public DateTime? DataDesativacao { get; set; }
}
