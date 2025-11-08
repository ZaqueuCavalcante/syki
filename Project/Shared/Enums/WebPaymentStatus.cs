namespace Exato.Shared.Enums;

/// <summary>
/// Status de um pagamento feito no Exato Web
/// </summary>
public enum WebPaymentStatus
{
    [Description("Desconhecido")]
    Desconhecido = 0,

    [Description("Ok")]
    Ok = 1,

    [Description("Pendente")]
    Pendente = 2,

    [Description("Recusado")]
    Recusado = 3,
}
