namespace Syki.Back.Errors;

public class InvalidDisciplinesList : SykiError
{
    public override string Code { get; set; } = nameof(InvalidDisciplinesList);
    public override string Message { get; set; } = "Lista de disciplinas inválida.";
}
public class InvalidAcademicPeriod : SykiError
{
    public override string Code { get; set; } = nameof(InvalidAcademicPeriod);
    public override string Message { get; set; } = "Período acadêmico inválido.";
}
public class InvalidAcademicPeriodStartDate : SykiError
{
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodStartDate);
    public override string Message { get; set; } = "Data de início de período acadêmico inválida.";
}
public class InvalidAcademicPeriodEndDate : SykiError
{
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodEndDate);
    public override string Message { get; set; } = "Data de fim de período acadêmico inválida.";
}
public class InvalidAcademicPeriodDates : SykiError
{
    public override string Code { get; set; } = nameof(InvalidAcademicPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}
public class InvalidCourseType : SykiError
{
    public override string Code { get; set; } = nameof(InvalidCourseType);
    public override string Message { get; set; } = "Tipo de curso inválido.";
}
public class InvalidShift : SykiError
{
    public override string Code { get; set; } = nameof(InvalidShift);
    public override string Message { get; set; } = "Turno inválido.";
}
public class InvalidEmail : SykiError
{
    public override string Code { get; set; } = nameof(InvalidEmail);
    public override string Message { get; set; } = "Email inválido.";
}
public class InvalidResetToken : SykiError
{
    public override string Code { get; set; } = nameof(InvalidResetToken);
    public override string Message { get; set; } = "Reset token inválido.";
}
public class InvalidSchedule : SykiError
{
    public override string Code { get; set; } = nameof(InvalidSchedule);
    public override string Message { get; set; } = "Horário inválido.";
}
public class InvalidDay : SykiError
{
    public override string Code { get; set; } = nameof(InvalidDay);
    public override string Message { get; set; } = "Dia inválido.";
}
public class InvalidHour : SykiError
{
    public override string Code { get; set; } = nameof(InvalidHour);
    public override string Message { get; set; } = "Hora inválida.";
}
public class InvalidEnrollmentPeriodDates : SykiError
{
    public override string Code { get; set; } = nameof(InvalidEnrollmentPeriodDates);
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim.";
}
public class InvalidMfaToken : SykiError
{
    public override string Code { get; set; } = nameof(InvalidMfaToken);
    public override string Message { get; set; } = "MFA token inválido.";
}
public class InvalidRegistrationToken : SykiError
{
    public override string Code { get; set; } = nameof(InvalidRegistrationToken);
    public override string Message { get; set; } = "Token de registro inválido.";
}
public class InvalidStudentsList : SykiError
{
    public override string Code { get; set; } = nameof(InvalidStudentsList);
    public override string Message { get; set; } = "Lista de alunos inválida.";
}
public class InvalidClassesList : SykiError
{
    public override string Code { get; set; } = nameof(InvalidClassesList);
    public override string Message { get; set; } = "Lista de turmas inválida.";
}
public class InvalidStudentClassNote : SykiError
{
    public override string Code { get; set; } = nameof(InvalidStudentClassNote);
    public override string Message { get; set; } = "Nota inválida.";
}
public class InvalidClassActivityWeight : SykiError
{
    public override string Code { get; set; } = nameof(InvalidClassActivityWeight);
    public override string Message { get; set; } = "Peso da atividade inválido.";
}
