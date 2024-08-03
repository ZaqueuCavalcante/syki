namespace Syki.Back.Errors;

public class SykiSuccess { }

public abstract class SykiError
{
    public abstract string Code { get; set; }
    public abstract string Message { get; set; }

    public SwaggerExample<ErrorOut> ToSwaggerExampleErrorOut()
    {
        return SwaggerExample.Create(Message, new ErrorOut { Message = Message });
    }
}

// NOT FOUND  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //
public class CourseOfferingNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CourseOfferingNotFound);
    public override string Message { get; set; } = "Oferta de curso não encontrada.";
}
public class CourseNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CourseNotFound);
    public override string Message { get; set; } = "Curso não encontrado.";
}
public class DisciplineNotFound : SykiError
{
    public override string Code { get; set; } = nameof(DisciplineNotFound);
    public override string Message { get; set; } = "Disciplina não encontrada.";
}
public class ClassNotFound : SykiError
{
    public override string Code { get; set; } = nameof(ClassNotFound);
    public override string Message { get; set; } = "Turma não encontrada.";
}
public class AcademicPeriodNotFound : SykiError
{
    public override string Code { get; set; } = nameof(AcademicPeriodNotFound);
    public override string Message { get; set; } = "Período acadêmico não encontrado.";
}
public class CampusNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CampusNotFound);
    public override string Message { get; set; } = "Campus não encontrado.";
}
public class CourseCurriculumNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CourseCurriculumNotFound);
    public override string Message { get; set; } = "Grade curricular não encontrada.";
}
public class InstitutionNotFound : SykiError
{
    public override string Code { get; set; } = nameof(InstitutionNotFound);
    public override string Message { get; set; } = "Instituição não encontrada.";
}
public class TeacherNotFound : SykiError
{
    public override string Code { get; set; } = nameof(TeacherNotFound);
    public override string Message { get; set; } = "Professor não encontrado.";
}
public class UserNotFound : SykiError
{
    public override string Code { get; set; } = nameof(UserNotFound);
    public override string Message { get; set; } = "Usuário não encontrado.";
}


// INVALID  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //
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
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim de período acadêmico.";
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
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim de período de matrícula.";
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


// OTHERS - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //
public class WeakPassword : SykiError
{
    public override string Code { get; set; } = nameof(WeakPassword);
    public override string Message { get; set; } = "Senha fraca.";
}
public class EmailAlreadyUsed : SykiError
{
    public override string Code { get; set; } = nameof(EmailAlreadyUsed);
    public override string Message { get; set; } = "Email já utilizado.";
}
public class ConflictingSchedules : SykiError
{
    public override string Code { get; set; } = nameof(ConflictingSchedules);
    public override string Message { get; set; } = "Horários conflitantes.";
}
public class UserAlreadyRegistered : SykiError
{
    public override string Code { get; set; } = nameof(UserAlreadyRegistered);
    public override string Message { get; set; } = "Usuário já cadastrado.";
}
public class AcademicPeriodAlreadyExists : SykiError
{
    public override string Code { get; set; } = nameof(AcademicPeriodAlreadyExists);
    public override string Message { get; set; } = "Já existe um período acadêmico com esse id.";
}
public class TeacherIsNotTheClassLeader : SykiError
{
    public override string Code { get; set; } = nameof(TeacherIsNotTheClassLeader);
    public override string Message { get; set; } = "O professor não é o titular da classe.";
}
