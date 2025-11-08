namespace Exato.Shared.Enums;

/// <summary>
/// Status do onboard no Exato Web.
/// </summary>
public enum ExatoWebOnboardStatus
{
    [Description("Não Completo")]
    Waiting = 0,

    [Description("Completo")]
    Completed = 1,

    [Description("Verificação Manual")]
    ManualVerification = 2,

    [Description("Não Aprovado")]
    NotApproved = 3,
}
