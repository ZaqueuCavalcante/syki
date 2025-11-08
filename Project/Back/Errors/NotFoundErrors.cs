namespace Exato.Back.Errors;

public class UserNotFound : ExatoError
{
    public static readonly UserNotFound I = new();
    public override string Code { get; set; } = nameof(UserNotFound);
    public override string Message { get; set; } = "Usuário não encontrado.";
}

public class UserAlreadyDeleted : ExatoError
{
    public static readonly UserAlreadyDeleted I = new();
    public override string Code { get; set; } = nameof(UserAlreadyDeleted);
    public override string Message { get; set; } = "Usuário já excluído anteriormente.";
}

public class RoleNotFound : ExatoError
{
    public static readonly RoleNotFound I = new();
    public override string Code { get; set; } = nameof(RoleNotFound);
    public override string Message { get; set; } = "Role não encontrada.";
}

public class RoleNameAlreadyExists : ExatoError
{
    public static readonly RoleNameAlreadyExists I = new();
    public override string Code { get; set; } = nameof(RoleNameAlreadyExists);
    public override string Message { get; set; } = "Já existe uma role com esse nome.";
}

public class EmpresaNaoEncontrada : ExatoError
{
    public static readonly EmpresaNaoEncontrada I = new();
    public override string Code { get; set; } = nameof(EmpresaNaoEncontrada);
    public override string Message { get; set; } = "Cliente não encontrado.";
}

public class TokenDeAcessoNaoEncontrado : ExatoError
{
    public static readonly TokenDeAcessoNaoEncontrado I = new();
    public override string Code { get; set; } = nameof(TokenDeAcessoNaoEncontrado);
    public override string Message { get; set; } = "Token de acesso não encontrado.";
}

public class DomainEventNotFound : ExatoError
{
    public static readonly DomainEventNotFound I = new();
    public override string Code { get; set; } = nameof(DomainEventNotFound);
    public override string Message { get; set; } = "Evento de domínio não encontrado.";
}

public class CommandNotFound : ExatoError
{
    public static readonly CommandNotFound I = new();
    public override string Code { get; set; } = nameof(CommandNotFound);
    public override string Message { get; set; } = "Comando não encontrado.";
}

public class CommandBatchNotFound : ExatoError
{
    public static readonly CommandBatchNotFound I = new();
    public override string Code { get; set; } = nameof(CommandBatchNotFound);
    public override string Message { get; set; } = "Lote de comandos não encontrado.";
}

public class CompanyNotFound : ExatoError
{
    public static readonly CompanyNotFound I = new();
    public override string Code { get; set; } = nameof(CompanyNotFound);
    public override string Message { get; set; } = "Company não encontrada.";
}

public class AuditTrailNotFound : ExatoError
{
    public static readonly AuditTrailNotFound I = new();
    public override string Code { get; set; } = nameof(AuditTrailNotFound);
    public override string Message { get; set; } = "Audit trail not found.";
}

public class WebUserCompanyNotFound : ExatoError
{
    public static readonly WebUserCompanyNotFound I = new();
    public override string Code { get; set; } = nameof(WebUserCompanyNotFound);
    public override string Message { get; set; } = "Vínculo entre usuário e empresa não encontrado no Exato Web.";
}

public class WebUserNotFound : ExatoError
{
    public static readonly WebUserNotFound I = new();
    public override string Code { get; set; } = nameof(WebUserNotFound);
    public override string Message { get; set; } = "Usuário não encontrado no Exato Web.";
}

public class WebCompanyNotFound : ExatoError
{
    public static readonly WebCompanyNotFound I = new();
    public override string Code { get; set; } = nameof(WebCompanyNotFound);
    public override string Message { get; set; } = "Empresa não encontrada no Exato Web.";
}
