namespace Exato.Shared.Enums;

/// <summary>
/// Métodos de Pagamento do Exato Web.
/// </summary>
public enum CompanyPaymentMode
{
    [Description("Pré-Pago")]
    PrePago = 0,

    [Description("Pós-Pago")]
    PosPago = 1,
}
