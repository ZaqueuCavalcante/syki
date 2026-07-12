namespace Estud.Back.Domain.Enums;

public enum DayType
{
    [Description("Dia letivo")]
    Default = 0,

    [Description("Férias")]
    Vacation = 1,

    [Description("Recesso")]
    Recess = 2,

    [Description("Feriado")]
    Holiday = 3,

    [Description("Fim de semana")]
    Weekend = 4,
}
