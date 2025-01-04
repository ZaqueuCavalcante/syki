using System.ComponentModel;

namespace Syki.Shared;

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
