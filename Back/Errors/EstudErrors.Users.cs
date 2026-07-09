namespace Estud.Back.Errors;

public class InvalidEmail : EstudError
{
    public static readonly InvalidEmail I = new();
    public override string Code { get; set; } = nameof(InvalidEmail);
    public override string Message { get; set; } = "Email inválido.";
}

public class EmailAlreadyUsed : EstudError
{
    public static readonly EmailAlreadyUsed I = new();
    public override string Code { get; set; } = nameof(EmailAlreadyUsed);
    public override string Message { get; set; } = "Email já utilizado.";
}

public class InvalidUserName : EstudError
{
    public static readonly InvalidUserName I = new();
    public override string Code { get; set; } = nameof(InvalidUserName);
    public override string Message { get; set; } = "Nome de usuário inválido.";
}
