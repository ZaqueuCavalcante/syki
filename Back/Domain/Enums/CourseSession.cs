namespace Syki.Back.Domain.Enums;

/// <summary>
/// Turno do Curso
/// </summary>
public enum CourseSession
{
    [Description("Matutino")]
    Morning = 0,

    [Description("Vespertino")]
    Afternoon = 1,

    [Description("Noturno")]
    Evening = 2,
}
