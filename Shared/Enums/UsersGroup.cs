using System.ComponentModel;

namespace Syki.Shared;

public enum UsersGroup
{
    [Description("Todos")]
    All,

    [Description("Alunos")]
    Students,

    [Description("Professores")]
    Teachers,
}
