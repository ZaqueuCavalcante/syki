using System.ComponentModel;

namespace Syki.Shared;

public enum Shift
{
    [Description("Matutino")]
    Matutino,

    [Description("Vespertino")]
    Vespertino,

    [Description("Noturno")]
    Noturno,
}
