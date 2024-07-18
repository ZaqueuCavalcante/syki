namespace Syki.Back.Exceptions;

public class CourseOfferingNotFound
{
    public string Message = "Oferta de curso não encontrada.";
}

public class CourseNotFound
{
    public string Message = "Curso não encontrado.";
}

public class InvalidDisciplinesList
{
    public string Message = "Lista de disciplinas inválida.";
}

public class DisciplineNotFound
{
    public string Message = "Disciplina não encontrada.";
}

public class AcademicPeriodNotFound
{
    public string Message = "Período acadêmico não encontrado.";
}

public class InvalidAcademicPeriod
{
    public string Message = "Período acadêmico inválido.";
}

public class InvalidAcademicPeriodStartDate
{
    public string Message = "Data de início de período acadêmico inválida.";
}

public class InvalidAcademicPeriodEndDate
{
    public string Message = "Data de fim de período acadêmico inválida.";
}

public class AcademicPeriodStartDateShouldBeLessThanEndDate
{
    public string Message = "A data de início deve ser menor que a de fim de período acadêmico.";
}

public class CampusNotFound
{
    public string Message = "Campus não encontrado.";
}

public class CourseCurriculumNotFound
{
    public string Message = "Grade curricular não encontrada.";
}

public class InstitutionNotFound
{
    public string Message = "Instituição não encontrada.";
}

public class WeakPassword
{
    public string Message = "Senha fraca.";
}

public class InvalidEmail
{
    public string Message = "Email inválido.";
}

public class EmailAlreadyUsed
{
    public string Message = "Email já utilizado.";
}

public class TeacherNotFound
{
    public string Message = "Professor não encontrado.";
}

public class UserNotFound
{
    public string Message = "Usuário não encontrado.";
}

public class InvalidResetToken
{
    public string Message = "Reset token inválido.";
}

public class InvalidSchedule
{
    public string Message = "Horário inválido.";
}

public class ConflictingSchedules
{
    public string Message = "Horários conflitantes.";
}

public class EnrollmentPeriodStartDateShouldBeLessThanEndDate
{
    public string Message = "A data de início deve ser menor que a de fim de período de matrícula.";
}

public class InvalidRegistrationToken
{
    public string Message = "Token de registro inválido.";
}

public class UserAlreadyRegistered
{
    public string Message = "Usuário já cadastrado.";
}

public class AcademicPeriodAlreadyExists
{
    public string Message = "Já existe um período acadêmico com esse id.";
}

public class InvalidMfaToken
{
    public string Message = "MFA token inválido.";
}

public class ClassNotFound
{
    public string Message = "Turma não encontrada.";
}
