namespace Syki.Back.Errors;

public class InvalidCampusName : SykiError
{
    public static readonly InvalidCampusName I = new();
    public override string Code { get; set; } = nameof(InvalidCampusName);
    public override string Message { get; set; } = "Nome de campus inválido.";
}
public class InvalidCampusCity : SykiError
{
    public static readonly InvalidCampusCity I = new();
    public override string Code { get; set; } = nameof(InvalidCampusCity);
    public override string Message { get; set; } = "Cidade do campus inválida.";
}
public class InvalidBrazilState : SykiError
{
    public static readonly InvalidBrazilState I = new();
    public override string Code { get; set; } = nameof(InvalidBrazilState);
    public override string Message { get; set; } = "Estado inválido.";
}
public class InvalidCampusCapacity : SykiError
{
    public static readonly InvalidCampusCapacity I = new();
    public override string Code { get; set; } = nameof(InvalidCampusCapacity);
    public override string Message { get; set; } = "Capacidade inválida (deve ser maior que zero).";
}

public class InvalidShift : SykiError
{
    public static readonly InvalidShift I = new();
    public override string Code { get; set; } = nameof(InvalidShift);
    public override string Message { get; set; } = "Turno inválido.";
}

public class InvalidCourseType : SykiError
{
    public static readonly InvalidCourseType I = new();
    public override string Code { get; set; } = nameof(InvalidCourseType);
    public override string Message { get; set; } = "Tipo de curso inválido.";
}

public class InvalidCourseName : SykiError
{
    public static readonly InvalidCourseName I = new();
    public override string Code { get; set; } = nameof(InvalidCourseName);
    public override string Message { get; set; } = "Nome de curso inválido.";
}






public class InvalidDisciplinesList : SykiError
{
    public static readonly InvalidDisciplinesList I = new();
    public override string Code { get; set; } = nameof(InvalidDisciplinesList);
    public override string Message { get; set; } = "Lista de disciplinas inválida.";
}
public class InvalidAcademicPeriod : SykiError
{
    public static readonly InvalidAcademicPeriod I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriod);
    public override string Message { get; set; } = "Período acadêmico inválido.";
}
public class InvalidAcademicPeriodStartDate : SykiError
{
    public static readonly InvalidAcademicPeriodStartDate I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodStartDate);
    public override string Message { get; set; } = "Data de início de período acadêmico inválida.";
}
public class InvalidAcademicPeriodEndDate : SykiError
{
    public static readonly InvalidAcademicPeriodEndDate I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodEndDate);
    public override string Message { get; set; } = "Data de fim de período acadêmico inválida.";
}
public class InvalidAcademicPeriodDates : SykiError
{
    public static readonly InvalidAcademicPeriodDates I = new();
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}


public class InvalidEmail : SykiError
{
    public static readonly InvalidEmail I = new();
    public override string Code { get; set; } = nameof(InvalidEmail);
    public override string Message { get; set; } = "Email inválido.";
}
public class InvalidResetToken : SykiError
{
    public static readonly InvalidResetToken I = new();
    public override string Code { get; set; } = nameof(InvalidResetToken);
    public override string Message { get; set; } = "Reset token inválido.";
}
public class InvalidSchedule : SykiError
{
    public static readonly InvalidSchedule I = new();
    public override string Code { get; set; } = nameof(InvalidSchedule);
    public override string Message { get; set; } = "Horário inválido.";
}
public class InvalidDay : SykiError
{
    public static readonly InvalidDay I = new();
    public override string Code { get; set; } = nameof(InvalidDay);
    public override string Message { get; set; } = "Dia inválido.";
}
public class InvalidHour : SykiError
{
    public static readonly InvalidHour I = new();
    public override string Code { get; set; } = nameof(InvalidHour);
    public override string Message { get; set; } = "Hora inválida.";
}
public class InvalidEnrollmentPeriodDates : SykiError
{
    public static readonly InvalidEnrollmentPeriodDates I = new();
    public override string Code { get; set; } = nameof(InvalidEnrollmentPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}
public class InvalidMfaToken : SykiError
{
    public static readonly InvalidMfaToken I = new();
    public override string Code { get; set; } = nameof(InvalidMfaToken);
    public override string Message { get; set; } = "MFA token inválido.";
}
public class InvalidRegistrationToken : SykiError
{
    public static readonly InvalidRegistrationToken I = new();
    public override string Code { get; set; } = nameof(InvalidRegistrationToken);
    public override string Message { get; set; } = "Token de registro inválido.";
}
public class InvalidStudentsList : SykiError
{
    public static readonly InvalidStudentsList I = new();
    public override string Code { get; set; } = nameof(InvalidStudentsList);
    public override string Message { get; set; } = "Lista de alunos inválida.";
}
public class InvalidClassesList : SykiError
{
    public static readonly InvalidClassesList I = new();
    public override string Code { get; set; } = nameof(InvalidClassesList);
    public override string Message { get; set; } = "Lista de turmas inválida.";
}
public class InvalidStudentClassNote : SykiError
{
    public static readonly InvalidStudentClassNote I = new();
    public override string Code { get; set; } = nameof(InvalidStudentClassNote);
    public override string Message { get; set; } = "Nota inválida.";
}
public class InvalidClassActivityWeight : SykiError
{
    public static readonly InvalidClassActivityWeight I = new();
    public override string Code { get; set; } = nameof(InvalidClassActivityWeight);
    public override string Message { get; set; } = "Peso da atividade inválido.";
}
public class InvalidStudentClassActivityNote : SykiError
{
    public static readonly InvalidStudentClassActivityNote I = new();
    public override string Code { get; set; } = nameof(InvalidStudentClassActivityNote);
    public override string Message { get; set; } = "Nota de atividade inválida.";
}
public class InvalidWebhookAuthentication : SykiError
{
    public static readonly InvalidWebhookAuthentication I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookAuthentication);
    public override string Message { get; set; } = "Autenticação de webhook inválida.";
}
public class InvalidWebhookEventsList : SykiError
{
    public static readonly InvalidWebhookEventsList I = new();
    public override string Code { get; set; } = nameof(InvalidWebhookEventsList);
    public override string Message { get; set; } = "Lista de eventos de webhook inválida.";
}
public class InvalidCampusList : SykiError
{
    public static readonly InvalidCampusList I = new();
    public override string Code { get; set; } = nameof(InvalidCampusList);
    public override string Message { get; set; } = "Lista de campus inválida.";
}
