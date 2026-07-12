namespace Estud.Back.Errors;

public class CalendarDayNotFound : EstudError
{
    public static readonly CalendarDayNotFound I = new();
    public override string Code { get; set; } = nameof(CalendarDayNotFound);
    public override string Message { get; set; } = "Dia de calendário não encontrado.";
}

public class InvalidCalendarDayDate : EstudError
{
    public static readonly InvalidCalendarDayDate I = new();
    public override string Code { get; set; } = nameof(InvalidCalendarDayDate);
    public override string Message { get; set; } = "Data de dia de calendário inválida.";
}

public class InvalidCalendarDayType : EstudError
{
    public static readonly InvalidCalendarDayType I = new();
    public override string Code { get; set; } = nameof(InvalidCalendarDayType);
    public override string Message { get; set; } = "Tipo de dia de calendário inválido.";
}

public class InvalidCalendarDayDescription : EstudError
{
    public static readonly InvalidCalendarDayDescription I = new();
    public override string Code { get; set; } = nameof(InvalidCalendarDayDescription);
    public override string Message { get; set; } = "Descrição de dia de calendário inválida.";
}

public class CalendarDayAlreadyExists : EstudError
{
    public static readonly CalendarDayAlreadyExists I = new();
    public override string Code { get; set; } = nameof(CalendarDayAlreadyExists);
    public override string Message { get; set; } = "Esse dia já foi customizado no calendário.";
}
