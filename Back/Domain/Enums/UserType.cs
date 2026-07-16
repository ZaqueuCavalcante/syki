namespace Estud.Back.Domain.Enums;

public enum UserType
{
    [Description("Gestor")]
    Manager = 0,

    [Description("Professor")]
    Teacher = 1,

    [Description("Aluno")]
    Student = 2,

    [Description("Responsável")]
    Parent = 3,
}
