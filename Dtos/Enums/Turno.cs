using System.ComponentModel;

namespace Syki.Dtos;

public enum Turno
{
    [Description("Matutino")]
    Matutino,

    [Description("Vespertino")]
    Vespertino,

    [Description("Noturno")]
    Noturno,
}
