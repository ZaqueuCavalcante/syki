using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Dia da semana
/// </summary>
public enum Day
{
    [Description("Segunda")]
    Monday,

    [Description("Terça")]
    Tuesday,

    [Description("Quarta")]
    Wednesday,

    [Description("Quinta")]
    Thursday,

    [Description("Sexta")]
    Friday,

    [Description("Sábado")]
    Saturday,
}
