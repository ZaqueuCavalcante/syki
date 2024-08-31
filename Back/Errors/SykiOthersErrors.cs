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
public class TeacherIsNotTheClassLeader : SykiError
{
    public override string Code { get; set; } = nameof(TeacherIsNotTheClassLeader);
    public override string Message { get; set; } = "O professor não é o titular da classe.";
}
public class EnrollmentPeriodAlreadyExists : SykiError
{
    public override string Code { get; set; } = nameof(EnrollmentPeriodAlreadyExists);
    public override string Message { get; set; } = "Já existe um período de matrícula para o período acadêmico informado.";
}
