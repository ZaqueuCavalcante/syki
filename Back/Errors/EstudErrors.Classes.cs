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

public class ClassAlreadyStarted : EstudError
{
    public static readonly ClassAlreadyStarted I = new();
    public override string Code { get; set; } = nameof(ClassAlreadyStarted);
    public override string Message { get; set; } = "A turma já foi iniciada.";
}

public class ClassWithoutTeachers : EstudError
{
    public static readonly ClassWithoutTeachers I = new();
    public override string Code { get; set; } = nameof(ClassWithoutTeachers);
    public override string Message { get; set; } = "A turma não possui professores atribuídos.";
}

public class ClassWithoutSchedules : EstudError
{
    public static readonly ClassWithoutSchedules I = new();
    public override string Code { get; set; } = nameof(ClassWithoutSchedules);
    public override string Message { get; set; } = "A turma não possui horários definidos.";
}

public class TeacherScheduleConflict : EstudError
{
    public static readonly TeacherScheduleConflict I = new();
    public override string Code { get; set; } = nameof(TeacherScheduleConflict);
    public override string Message { get; set; } = "Conflito de horário com outra turma do professor.";
}

public class ScheduleTeacherRequired : EstudError
{
    public static readonly ScheduleTeacherRequired I = new();
    public override string Code { get; set; } = nameof(ScheduleTeacherRequired);
    public override string Message { get; set; } = "Cada horário deve ter um professor definido.";
}

public class InvalidScheduleTeacher : EstudError
{
    public static readonly InvalidScheduleTeacher I = new();
    public override string Code { get; set; } = nameof(InvalidScheduleTeacher);
    public override string Message { get; set; } = "Professor do horário não pertence à turma.";
}

public class InvalidClassVacancies : EstudError
{
    public static readonly InvalidClassVacancies I = new();
    public override string Code { get; set; } = nameof(InvalidClassVacancies);
    public override string Message { get; set; } = "Número de vagas inválido.";
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

public class ClassLessonNotFound : EstudError
{
    public static readonly ClassLessonNotFound I = new();
    public override string Code { get; set; } = nameof(ClassLessonNotFound);
    public override string Message { get; set; } = "Aula não encontrada.";
}

public class InvalidStudentsList : EstudError
{
    public static readonly InvalidStudentsList I = new();
    public override string Code { get; set; } = nameof(InvalidStudentsList);
    public override string Message { get; set; } = "Lista de alunos inválida.";
}
