using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Turno
/// </summary>
public enum Shift
{
    [Description("Matutino")]
    Matutino,

    [Description("Vespertino")]
    Vespertino,

    [Description("Noturno")]
    Noturno,
}
