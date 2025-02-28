using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Papel de um Usuário
/// </summary>
public enum UserRole
{
    [Description("Adm")]
    Adm,

    [Description("Acadêmico")]
    Academic,

    [Description("Professor")]
    Teacher,

    [Description("Aluno")]
    Student,
}
