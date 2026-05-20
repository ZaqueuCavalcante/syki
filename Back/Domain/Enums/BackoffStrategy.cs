namespace Syki.Back.Domain.Enums;

/// <summary>
/// Estratégia de backoff para retentativas de comandos
/// </summary>
public enum BackoffStrategy
{
    [Description("Sem backoff")]
    None = 0,

    [Description("Backoff exponencial")]
    Exponential = 1,

    [Description("Backoff linear")]
    Linear = 2,

    [Description("Backoff fixo")]
    Fixed = 3,
}
