namespace Estud.Back.Errors;

public class InvalidDay : EstudError
{
    public static readonly InvalidDay I = new();
    public override string Code { get; set; } = nameof(InvalidDay);
    public override string Message { get; set; } = "Dia inválido.";
}

public class InvalidHour : EstudError
{
    public static readonly InvalidHour I = new();
    public override string Code { get; set; } = nameof(InvalidHour);
    public override string Message { get; set; } = "Hora inválida.";
}

public class InvalidSchedule : EstudError
{
    public static readonly InvalidSchedule I = new();
    public override string Code { get; set; } = nameof(InvalidSchedule);
    public override string Message { get; set; } = "Horário inválido.";
}

public class ConflictingSchedules : EstudError
{
    public static readonly ConflictingSchedules I = new();
    public override string Code { get; set; } = nameof(ConflictingSchedules);
    public override string Message { get; set; } = "Horários conflitantes.";
}

public class InvalidClassActivityWeight : EstudError
{
    public static readonly InvalidClassActivityWeight I = new();
    public override string Code { get; set; } = nameof(InvalidClassActivityWeight);
    public override string Message { get; set; } = "Peso da atividade inválido.";
}

public class InvalidStudentClassNote : EstudError
{
    public static readonly InvalidStudentClassNote I = new();
    public override string Code { get; set; } = nameof(InvalidStudentClassNote);
    public override string Message { get; set; } = "Nota da atividade inválida.";
}
