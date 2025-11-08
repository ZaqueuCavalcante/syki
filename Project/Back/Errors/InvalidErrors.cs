namespace Exato.Back.Errors;

public class InvalidUserName : ExatoError
{
    public static readonly InvalidUserName I = new();
    public override string Code { get; set; } = nameof(InvalidUserName);
    public override string Message { get; set; } = "Nome de usuário inválido.";
}

public class InvalidEmail : ExatoError
{
    public static readonly InvalidEmail I = new();
    public override string Code { get; set; } = nameof(InvalidEmail);
    public override string Message { get; set; } = "Email inválido.";
}

public class InvalidResetPasswordToken : ExatoError
{
    public static readonly InvalidResetPasswordToken I = new();
    public override string Code { get; set; } = nameof(InvalidResetPasswordToken);
    public override string Message { get; set; } = "Token de reset de senha inválido.";
}

public class InvalidCpf : ExatoError
{
    public static readonly InvalidCpf I = new();
    public override string Code { get; set; } = nameof(InvalidCpf);
    public override string Message { get; set; } = "CPF inválido.";
}

public class InvalidCnpj : ExatoError
{
    public static readonly InvalidCnpj I = new();
    public override string Code { get; set; } = nameof(InvalidCnpj);
    public override string Message { get; set; } = "CNPJ inválido.";
}

public class NomeDeEmpresaInvalido : ExatoError
{
    public static readonly NomeDeEmpresaInvalido I = new();
    public override string Code { get; set; } = nameof(NomeDeEmpresaInvalido);
    public override string Message { get; set; } = "Nome de empresa inválido.";
}

public class Invalid2faToken : ExatoError
{
    public static readonly Invalid2faToken I = new();
    public override string Code { get; set; } = nameof(Invalid2faToken);
    public override string Message { get; set; } = "2FA token inválido.";
}

public class RelatorioInvalido : ExatoError
{
    public static readonly RelatorioInvalido I = new();
    public override string Code { get; set; } = nameof(RelatorioInvalido);
    public override string Message { get; set; } = "Relatório inválido.";
}

public class MatrizInvalida : ExatoError
{
    public static readonly MatrizInvalida I = new();
    public override string Code { get; set; } = nameof(MatrizInvalida);
    public override string Message { get; set; } = "Matriz inválida.";
}

public class MetodoDePagamentoInvalido : ExatoError
{
    public static readonly MetodoDePagamentoInvalido I = new();
    public override string Code { get; set; } = nameof(MetodoDePagamentoInvalido);
    public override string Message { get; set; } = "Método de pagamento inválido.";
}

public class LimiteDeConsultasSemanalInvalido : ExatoError
{
    public static readonly LimiteDeConsultasSemanalInvalido I = new();
    public override string Code { get; set; } = nameof(LimiteDeConsultasSemanalInvalido);
    public override string Message { get; set; } = "Limite de consultas semanal inválido.";
}

public class NivelDeAcessoADadosInvalido : ExatoError
{
    public static readonly NivelDeAcessoADadosInvalido I = new();
    public override string Code { get; set; } = nameof(NivelDeAcessoADadosInvalido);
    public override string Message { get; set; } = "Nível de acesso a dados inválido.";
}

public class NomeDeTokenInvalido : ExatoError
{
    public static readonly NomeDeTokenInvalido I = new();
    public override string Code { get; set; } = nameof(NomeDeTokenInvalido);
    public override string Message { get; set; } = "Nome de token inválido.";
}

public class DescricaoDeTokenInvalida : ExatoError
{
    public static readonly DescricaoDeTokenInvalida I = new();
    public override string Code { get; set; } = nameof(DescricaoDeTokenInvalida);
    public override string Message { get; set; } = "Descrição de token inválida.";
}

public class ValidadeDeTokenInvalida : ExatoError
{
    public static readonly ValidadeDeTokenInvalida I = new();
    public override string Code { get; set; } = nameof(ValidadeDeTokenInvalida);
    public override string Message { get; set; } = "Validade do token inválida.";
}

public class NomeDeUsuarioInvalido : ExatoError
{
    public static readonly NomeDeUsuarioInvalido I = new();
    public override string Code { get; set; } = nameof(NomeDeUsuarioInvalido);
    public override string Message { get; set; } = "Nome de usuário inválido.";
}

public class ValorEmReaisInvalido : ExatoError
{
    public static readonly ValorEmReaisInvalido I = new();
    public override string Code { get; set; } = nameof(ValorEmReaisInvalido);
    public override string Message { get; set; } = "Valor em R$ inválido.";
}

public class ValorEmCreditosInvalido : ExatoError
{
    public static readonly ValorEmCreditosInvalido I = new();
    public override string Code { get; set; } = nameof(ValorEmCreditosInvalido);
    public override string Message { get; set; } = "Valor em créditos inválido.";
}

public class InvalidPhone : ExatoError
{
    public static readonly InvalidPhone I = new();
    public override string Code { get; set; } = nameof(InvalidPhone);
    public override string Message { get; set; } = "Telefone inválido.";
}

public class ListaDeEmpresasInvalida : ExatoError
{
    public static readonly ListaDeEmpresasInvalida I = new();
    public override string Code { get; set; } = nameof(ListaDeEmpresasInvalida);
    public override string Message { get; set; } = "Lista de empresas inválida.";
}

public class InvalidRoleName : ExatoError
{
    public static readonly InvalidRoleName I = new();
    public override string Code { get; set; } = nameof(InvalidRoleName);
    public override string Message { get; set; } = "Nome de role inválido.";
}

public class InvalidRoleDescription : ExatoError
{
    public static readonly InvalidRoleDescription I = new();
    public override string Code { get; set; } = nameof(InvalidRoleDescription);
    public override string Message { get; set; } = "Descrição de role inválida.";
}

public class InvalidFeaturesList : ExatoError
{
    public static readonly InvalidFeaturesList I = new();
    public override string Code { get; set; } = nameof(InvalidFeaturesList);
    public override string Message { get; set; } = "Lista de features inválida.";
}
