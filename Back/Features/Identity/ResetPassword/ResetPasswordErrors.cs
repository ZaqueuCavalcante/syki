namespace Estud.Back.Features.Identity.ResetPassword;

public class InvalidResetPasswordToken : EstudError
{
    public static readonly InvalidResetPasswordToken I = new();
    public override string Code { get; set; } = nameof(InvalidResetPasswordToken);
    public override string Message { get; set; } = "Token de reset de senha inválido.";
}

public class WeakPassword : EstudError
{
    public static readonly WeakPassword I = new();
    public override string Code { get; set; } = nameof(WeakPassword);
    public override string Message { get; set; } = "Senha fraca.";
}
