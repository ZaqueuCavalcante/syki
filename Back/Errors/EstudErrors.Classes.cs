namespace Estud.Back.Errors;

public class ClassNotFound : EstudError
{
    public static readonly ClassNotFound I = new();
    public override string Code { get; set; } = nameof(ClassNotFound);
    public override string Message { get; set; } = "Turma não encontrada.";
}

public class ClassMustBeOnPreEnrollment : EstudError
{
    public static readonly ClassMustBeOnPreEnrollment I = new();
    public override string Code { get; set; } = nameof(ClassMustBeOnPreEnrollment);
    public override string Message { get; set; } = "A turma deve estar em pré-matrícula.";
}

public class ClassMustBeOnEnrollment : EstudError
{
    public static readonly ClassMustBeOnEnrollment I = new();
    public override string Code { get; set; } = nameof(ClassMustBeOnEnrollment);
    public override string Message { get; set; } = "A turma deve estar em matrícula.";
}

public class NoVacanciesInClass : EstudError
{
    public static readonly NoVacanciesInClass I = new();
    public override string Code { get; set; } = nameof(NoVacanciesInClass);
    public override string Message { get; set; } = "A turma não possui vagas disponíveis.";
}

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

public class ClassActivityNotFound : EstudError
{
    public static readonly ClassActivityNotFound I = new();
    public override string Code { get; set; } = nameof(ClassActivityNotFound);
    public override string Message { get; set; } = "Atividade não encontrada.";
}

public class ClassActivityWorkNotFound : EstudError
{
    public static readonly ClassActivityWorkNotFound I = new();
    public override string Code { get; set; } = nameof(ClassActivityWorkNotFound);
    public override string Message { get; set; } = "Entrega da atividade não encontrada.";
}

public class InvalidClassActivityWorkLink : EstudError
{
    public static readonly InvalidClassActivityWorkLink I = new();
    public override string Code { get; set; } = nameof(InvalidClassActivityWorkLink);
    public override string Message { get; set; } = "Link da entrega inválido.";
}
