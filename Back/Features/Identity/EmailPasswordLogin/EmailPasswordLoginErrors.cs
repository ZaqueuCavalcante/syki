namespace Estud.Back.Features.Identity.EmailPasswordLogin;

public class LoginWrongEmailOrPassword : EstudError
{
    public static readonly LoginWrongEmailOrPassword I = new();
    public override string Code { get; set; } = nameof(LoginWrongEmailOrPassword);
    public override string Message { get; set; } = "Email ou senha incorretos.";
}

public class LoginUserLockedOut : EstudError
{
    public static readonly LoginUserLockedOut I = new();
    public override string Code { get; set; } = nameof(LoginUserLockedOut);
    public override string Message { get; set; } = "Usuário bloqueado temporariamente devido a tentativas de login inválidas.";
}

public class LoginRequiresTwoFactor : EstudError
{
    public static readonly LoginRequiresTwoFactor I = new();
    public override string Code { get; set; } = nameof(LoginRequiresTwoFactor);
    public override string Message { get; set; } = "Utilize o segundo fator de autenticação para realizar login.";
}

public class LoginTwoFactorEnforced : EstudError
{
    public static readonly LoginTwoFactorEnforced I = new();
    public override string Code { get; set; } = nameof(LoginTwoFactorEnforced);
    public override string Message { get; set; } = "Autenticação de dois fatores é obrigatória. Configure o 2FA para realizar login.";
}

public class SsoLoginRequired : EstudError
{
    public static readonly SsoLoginRequired I = new();
    public override string Code { get; set; } = nameof(SsoLoginRequired);
    public override string Message { get; set; } = "Login via SSO é obrigatório para este domínio.";
}
