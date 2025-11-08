namespace Exato.Shared.Enums;

/// <summary>
/// Balance Types do Intelligence. <br/>
/// Usados no Faturamento, tabela public.cliente, coluna balance_type.
/// </summary>
public enum BalanceType
{
    /// <summary>
    /// Vai consumir o valor da coluna cliente.balance_in_brl
    /// </summary>
    [Description("R$")]
    Reais = 1,

    /// <summary>
    /// Vai consumir da coluna cliente.saldo
    /// </summary>
    [Description("Créditos")]
    Creditos = 2,
}
