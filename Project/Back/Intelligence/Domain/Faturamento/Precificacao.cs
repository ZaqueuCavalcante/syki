namespace Exato.Back.Intelligence.Domain.Faturamento;

/// <summary>
/// Preço por tipo de consulta em um plano, segmentado por faixa.
/// </summary>
public class Precificacao
{
    public int Id { get; set; }

    /// <summary>
    /// FK para planos.
    /// </summary>
    public int PlanoId { get; set; }

    /// <summary>
    /// FK para consulta_tipo.
    /// </summary>
    public int ConsultaTipoId { get; set; }

    /// <summary>
    /// Identificador de faixa associada.
    /// </summary>
    public int FaixasId { get; set; }

    /// <summary>
    /// Início da subfaixa interna (inclusive).
    /// </summary>
    public int InicioFaixa { get; set; }

    /// <summary>
    /// Fim da subfaixa interna (inclusive); NULL = sem limite.
    /// </summary>
    public int? FimFaixa { get; set; }

    /// <summary>
    /// Preço unitário aplicável.
    /// </summary>
    public decimal ValorUnitario { get; set; }
}
