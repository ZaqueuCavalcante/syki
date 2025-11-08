namespace Exato.Shared.Enums;

/// <summary>
/// Métodos de Pagamento do Intelligence. <br/>
/// Usados no Faturamento, coluna "public.cliente.faturamento_tipo_id".
/// </summary>
public enum MetodoDePagamento
{
    /// <summary>
    /// Precisa ter saldo na coluna "public.cliente.balance_in_brl" ou na "cliente.saldo" <br/>
    /// dependendo pra onde a "public.cliente.balance_type" está apontando. <br/>
    /// Uma dessas colunas tem que estar maior que zero para que o cliente consiga consultar.
    /// </summary>
    [Description("Pré-Pago")]
    PrePago = 1,

    /// <summary>
    /// Consegue consultar infinitamente se não tiver limite de transações ou de créditos.
    /// </summary>
    [Description("Pós-Pago")]
    PosPago = 2,
}
