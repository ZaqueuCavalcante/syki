namespace Syki.Back.Domain.Enums;

public enum UserActivitySeverity : short
{
    [Description("Successo")]
    Success = 0,

    [Description("Informação")]
    Info = 1,

    [Description("Erro")]
    Error = 2,
}
