namespace Exato.Shared.Enums;

/// <summary>
/// Provider de pagamento do Exato Web
/// </summary>
public enum WebPaymentProvider
{
    [Description("PagSeguro")]
    PagSeguro = 1,

    [Description("PagarMe")]
    PagarMe = 2,
}
