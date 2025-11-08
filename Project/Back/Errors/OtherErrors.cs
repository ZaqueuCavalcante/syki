namespace Exato.Back.Errors;

public class EmailAlreadyUsed : ExatoError
{
    public static readonly EmailAlreadyUsed I = new();
    public override string Code { get; set; } = nameof(EmailAlreadyUsed);
    public override string Message { get; set; } = "Email já utilizado.";
}

public class WeakPassword : ExatoError
{
    public static readonly WeakPassword I = new();
    public override string Code { get; set; } = nameof(WeakPassword);
    public override string Message { get; set; } = "Senha fraca.";
}

public class LoginWrongEmailOrPassword : ExatoError
{
    public static readonly LoginWrongEmailOrPassword I = new();
    public override string Code { get; set; } = nameof(LoginWrongEmailOrPassword);
    public override string Message { get; set; } = "Email ou senha incorretos.";
}

public class LoginRequiresTwoFactor : ExatoError
{
    public static readonly LoginRequiresTwoFactor I = new();
    public override string Code { get; set; } = nameof(LoginRequiresTwoFactor);
    public override string Message { get; set; } = "Utilize o segundo fator de autenticação para realizar login.";
}

public class ApenasMatrizesPodemSerFaturadas : ExatoError
{
    public static readonly ApenasMatrizesPodemSerFaturadas I = new();
    public override string Code { get; set; } = nameof(ApenasMatrizesPodemSerFaturadas);
    public override string Message { get; set; } = "Apenas matrizes podem ser faturadas.";
}

public class ApenasMatrizesPosPagasPodemSerFaturadas : ExatoError
{
    public static readonly ApenasMatrizesPosPagasPodemSerFaturadas I = new();
    public override string Code { get; set; } = nameof(ApenasMatrizesPosPagasPodemSerFaturadas);
    public override string Message { get; set; } = "Apenas matrizes pós-pagas podem ser faturadas.";
}

public class ApenasMatrizesPodemPossuirVendedorResponsavel : ExatoError
{
    public static readonly ApenasMatrizesPodemPossuirVendedorResponsavel I = new();
    public override string Code { get; set; } = nameof(ApenasMatrizesPodemPossuirVendedorResponsavel);
    public override string Message { get; set; } = "Apenas matrizes podem possuir vendedor responsável.";
}

public class UsuarioJaVinculadoAEmpresaNoIntelligence : ExatoError
{
    public static readonly UsuarioJaVinculadoAEmpresaNoIntelligence I = new();
    public override string Code { get; set; } = nameof(UsuarioJaVinculadoAEmpresaNoIntelligence);
    public override string Message { get; set; } = "Usuário já vinculado à empresa no Intelligence.";
}

public class TokenDeUsuarioJaVinculadoAEmpresaNoIntelligence : ExatoError
{
    public static readonly TokenDeUsuarioJaVinculadoAEmpresaNoIntelligence I = new();
    public override string Code { get; set; } = nameof(TokenDeUsuarioJaVinculadoAEmpresaNoIntelligence);
    public override string Message { get; set; } = "Token de usuário já vinculado à empresa no Intelligence.";
}

public class UsuarioJaVinculadoAEmpresaNoExatoWeb : ExatoError
{
    public static readonly UsuarioJaVinculadoAEmpresaNoExatoWeb I = new();
    public override string Code { get; set; } = nameof(UsuarioJaVinculadoAEmpresaNoExatoWeb);
    public override string Message { get; set; } = "Usuário já vinculado à empresa no Exato Web.";
}

public class VinculoEmpresaUsuarioNaoExiste : ExatoError
{
    public static readonly VinculoEmpresaUsuarioNaoExiste I = new();
    public override string Code { get; set; } = nameof(VinculoEmpresaUsuarioNaoExiste);
    public override string Message { get; set; } = "Vínculo usuário-empresa não existe.";
}

public class OnlyRootCommandsCanBeReprocessed : ExatoError
{
    public static readonly OnlyRootCommandsCanBeReprocessed I = new();
    public override string Code { get; set; } = nameof(OnlyRootCommandsCanBeReprocessed);
    public override string Message { get; set; } = "Apenas o comando original pode ser reprocessado.";
}
