namespace Syki.Back.Exceptions;

public class SykiSuccess { }

public abstract class SykiError
{
    public abstract string Message { get; set; }

    public SwaggerExample<ErrorOut> ToSwaggerExampleErrorOut()
    {
        return SwaggerExample.Create(Message, new ErrorOut { Message = Message });
    }
}

public class CourseOfferingNotFound : SykiError
{
    public override string Message { get; set; } = "Oferta de curso não encontrada.";
}

public class CourseNotFound : SykiError
{
    public override string Message { get; set; } = "Curso não encontrado.";
}

public class InvalidDisciplinesList : SykiError
{
    public override string Message { get; set; } = "Lista de disciplinas inválida.";
}

public class DisciplineNotFound : SykiError
{
    public override string Message { get; set; } = "Disciplina não encontrada.";
}

public class AcademicPeriodNotFound : SykiError
{
    public override string Message { get; set; } = "Período acadêmico não encontrado.";
}

public class InvalidAcademicPeriod : SykiError
{
    public override string Message { get; set; } = "Período acadêmico inválido.";
}

public class InvalidAcademicPeriodStartDate : SykiError
{
    public override string Message { get; set; } = "Data de início de período acadêmico inválida.";
}

public class InvalidAcademicPeriodEndDate : SykiError
{
    public override string Message { get; set; } = "Data de fim de período acadêmico inválida.";
}

public class AcademicPeriodStartDateShouldBeLessThanEndDate : SykiError
{
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim de período acadêmico.";
}

public class CampusNotFound : SykiError
{
    public override string Message { get; set; } = "Campus não encontrado.";
}

public class CourseCurriculumNotFound : SykiError
{
    public override string Message { get; set; } = "Grade curricular não encontrada.";
}

public class InstitutionNotFound : SykiError
{
    public override string Message { get; set; } = "Instituição não encontrada.";
}

public class WeakPassword : SykiError
{
    public override string Message { get; set; } = "Senha fraca.";
}

public class InvalidEmail : SykiError
{
    public override string Message { get; set; } = "Email inválido.";
}

public class EmailAlreadyUsed : SykiError
{
    public override string Message { get; set; } = "Email já utilizado.";
}

public class TeacherNotFound : SykiError
{
    public override string Message { get; set; } = "Professor não encontrado.";
}

public class UserNotFound : SykiError
{
    public override string Message { get; set; } = "Usuário não encontrado.";
}

public class InvalidResetToken : SykiError
{
    public override string Message { get; set; } = "Reset token inválido.";
}

public class InvalidSchedule : SykiError
{
    public override string Message { get; set; } = "Horário inválido.";
}

public class ConflictingSchedules : SykiError
{
    public override string Message { get; set; } = "Horários conflitantes.";
}

public class EnrollmentPeriodStartDateShouldBeLessThanEndDate : SykiError
{
    public override string Message { get; set; } = "A data de início deve ser menor que a de fim de período de matrícula.";
}

public class InvalidRegistrationToken : SykiError
{
    public override string Message { get; set; } = "Token de registro inválido.";
}

public class UserAlreadyRegistered : SykiError
{
    public override string Message { get; set; } = "Usuário já cadastrado.";
}

public class AcademicPeriodAlreadyExists : SykiError
{
    public override string Message { get; set; } = "Já existe um período acadêmico com esse id.";
}

public class InvalidMfaToken : SykiError
{
    public override string Message { get; set; } = "MFA token inválido.";
}

public class ClassNotFound : SykiError
{
    public override string Message { get; set; } = "Turma não encontrada.";
}
