using System.ComponentModel;

namespace Syki.Shared;

public enum Turno
{
    [Description("Matutino")]
    Matutino,

    [Description("Vespertino")]
    Vespertino,

    [Description("Noturno")]
    Noturno,
}
