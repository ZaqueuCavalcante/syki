using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Grupo de Usuários
/// </summary>
public enum UsersGroup
{
    [Description("Todos")]
    All,

    [Description("Alunos")]
    Students,

    [Description("Professores")]
    Teachers,
}
