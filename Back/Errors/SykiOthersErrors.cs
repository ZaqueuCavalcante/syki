namespace Syki.Back.Errors;

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
public class EnrollmentPeriodAlreadyExists : SykiError
{
    public override string Code { get; set; } = nameof(EnrollmentPeriodAlreadyExists);
    public override string Message { get; set; } = "Já existe um período de matrícula para o período acadêmico informado.";
}
public class AllClassesMustHaveOnPreEnrollmentStatus : SykiError
{
    public override string Code { get; set; } = nameof(AllClassesMustHaveOnPreEnrollmentStatus);
    public override string Message { get; set; } = "Todas as turmas precisam ter o status de Pré-matrícula.";
}
public class EnrollmentPeriodNotStarted : SykiError
{
    public override string Code { get; set; } = nameof(EnrollmentPeriodNotStarted);
    public override string Message { get; set; } = "Período de matrícula não iniciado.";
}
public class EnrollmentPeriodFinalized : SykiError
{
    public override string Code { get; set; } = nameof(EnrollmentPeriodFinalized);
    public override string Message { get; set; } = "Período de matrícula finalizado.";
}
public class ClassMustHaveOnEnrollmentStatus : SykiError
{
    public override string Code { get; set; } = nameof(ClassMustHaveOnEnrollmentStatus);
    public override string Message { get; set; } = "A turma precisam ter o status de Matrícula para ser iniciada.";
}
public class EnrollmentPeriodMustBeFinalized : SykiError
{
    public override string Code { get; set; } = nameof(EnrollmentPeriodMustBeFinalized);
    public override string Message { get; set; } = "O período de matrícula precisa estar finalizado.";
}
public class AllClassLessonsMustHaveFinalizedStatus : SykiError
{
    public override string Code { get; set; } = nameof(AllClassLessonsMustHaveFinalizedStatus);
    public override string Message { get; set; } = "Todas as aulas da turma precisam estar concluídas.";
}
public class ClassMustHaveStartedStatus : SykiError
{
    public override string Code { get; set; } = nameof(ClassMustHaveStartedStatus);
    public override string Message { get; set; } = "A turma precisa ter o status de Iniciada para ser finalizada.";
}

public class LoginWrongEmailOrPassword : SykiError
{
    public override string Code { get; set; } = nameof(LoginWrongEmailOrPassword);
    public override string Message { get; set; } = "Email ou senha incorretos.";
}
public class LoginRequiresTwoFactor : SykiError
{
    public override string Code { get; set; } = nameof(LoginRequiresTwoFactor);
    public override string Message { get; set; } = "Utilize o segundo fator de autenticação para realizar login.";
}
public class LoginWrongMfaToken : SykiError
{
    public override string Code { get; set; } = nameof(LoginWrongMfaToken);
    public override string Message { get; set; } = "Token incorreto.";
}
public class OnlyRootCommandsCanBeReprocessed : SykiError
{
    public override string Code { get; set; } = nameof(OnlyRootCommandsCanBeReprocessed);
    public override string Message { get; set; } = "Apenas o comando original pode ser reprocessado.";
}
public class TeacherNotAssignedToCampus : SykiError
{
    public override string Code { get; set; } = nameof(TeacherNotAssignedToCampus);
    public override string Message { get; set; } = "Professor não vinculado ao campus.";
}
public class TeacherNotAssignedToDiscipline : SykiError
{
    public override string Code { get; set; } = nameof(TeacherNotAssignedToDiscipline);
    public override string Message { get; set; } = "Professor não vinculado a disciplina.";
}
