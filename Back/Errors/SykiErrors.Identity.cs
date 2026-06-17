namespace Syki.Back.Errors;

public class InvalidTwoFactorToken : SykiError
{
    public static readonly InvalidTwoFactorToken I = new();
    public override string Code { get; set; } = nameof(InvalidTwoFactorToken);
    public override string Message { get; set; } = "2FA token inválido.";
}

public class InvalidRoleName : SykiError
{
    public static readonly InvalidRoleName I = new();
    public override string Code { get; set; } = nameof(InvalidRoleName);
    public override string Message { get; set; } = "Nome de role inválido.";
}

public class InvalidRoleDescription : SykiError
{
    public static readonly InvalidRoleDescription I = new();
    public override string Code { get; set; } = nameof(InvalidRoleDescription);
    public override string Message { get; set; } = "Descrição de role inválida.";
}

public class InvalidPermissionsList : SykiError
{
    public static readonly InvalidPermissionsList I = new();
    public override string Code { get; set; } = nameof(InvalidPermissionsList);
    public override string Message { get; set; } = "Lista de permissions inválida.";
}

public class InvalidSsoProviderType : SykiError
{
    public static readonly InvalidSsoProviderType I = new();
    public override string Code { get; set; } = nameof(InvalidSsoProviderType);
    public override string Message { get; set; } = "Tipo de provedor SSO inválido.";
}

public class InvalidSsoAuthority : SykiError
{
    public static readonly InvalidSsoAuthority I = new();
    public override string Code { get; set; } = nameof(InvalidSsoAuthority);
    public override string Message { get; set; } = "URL do provedor SSO inválida.";
}

public class InvalidSsoClientId : SykiError
{
    public static readonly InvalidSsoClientId I = new();
    public override string Code { get; set; } = nameof(InvalidSsoClientId);
    public override string Message { get; set; } = "Client ID inválido.";
}

public class InvalidSsoClientSecret : SykiError
{
    public static readonly InvalidSsoClientSecret I = new();
    public override string Code { get; set; } = nameof(InvalidSsoClientSecret);
    public override string Message { get; set; } = "Client Secret inválido.";
}

public class SsoAuthorityMustBeHttps : SykiError
{
    public static readonly SsoAuthorityMustBeHttps I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityMustBeHttps);
    public override string Message { get; set; } = "URL do provedor SSO deve usar HTTPS.";
}

public class SsoAuthorityLocalhostNotAllowed : SykiError
{
    public static readonly SsoAuthorityLocalhostNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityLocalhostNotAllowed);
    public override string Message { get; set; } = "Localhost não é permitido em produção.";
}

public class SsoAuthorityPrivateIpNotAllowed : SykiError
{
    public static readonly SsoAuthorityPrivateIpNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityPrivateIpNotAllowed);
    public override string Message { get; set; } = "IP privado não é permitido em produção.";
}

public class SsoAuthorityHasUserInfo : SykiError
{
    public static readonly SsoAuthorityHasUserInfo I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityHasUserInfo);
    public override string Message { get; set; } = "URL do provedor SSO não pode conter credenciais.";
}

public class SsoAuthorityLinkLocalNotAllowed : SykiError
{
    public static readonly SsoAuthorityLinkLocalNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityLinkLocalNotAllowed);
    public override string Message { get; set; } = "IP link-local não é permitido.";
}

public class SsoAuthorityLoopbackNotAllowed : SykiError
{
    public static readonly SsoAuthorityLoopbackNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityLoopbackNotAllowed);
    public override string Message { get; set; } = "IP de loopback não é permitido em produção.";
}

public class SsoConfigurationNotFound : SykiError
{
    public static readonly SsoConfigurationNotFound I = new();
    public override string Code { get; set; } = nameof(SsoConfigurationNotFound);
    public override string Message { get; set; } = "Configuração SSO não encontrada.";
}

public class SsoNotConfiguredForDomain : SykiError
{
    public static readonly SsoNotConfiguredForDomain I = new();
    public override string Code { get; set; } = nameof(SsoNotConfiguredForDomain);
    public override string Message { get; set; } = "SSO não está configurado para este domínio.";
}

public class SsoAuthenticationFailed : SykiError
{
    public static readonly SsoAuthenticationFailed I = new();
    public override string Code { get; set; } = nameof(SsoAuthenticationFailed);
    public override string Message { get; set; } = "Falha na autenticação SSO.";
}

public class SsoLoginUserNotFound : SykiError
{
    public static readonly SsoLoginUserNotFound I = new();
    public override string Code { get; set; } = nameof(SsoLoginUserNotFound);
    public override string Message { get; set; } = "Usuário não pertence à organização.";
}

public class RoleNotFound : SykiError
{
    public static readonly RoleNotFound I = new();
    public override string Code { get; set; } = nameof(RoleNotFound);
    public override string Message { get; set; } = "Role não encontrada.";
}

public class RoleNameAlreadyExists : SykiError
{
    public static readonly RoleNameAlreadyExists I = new();
    public override string Code { get; set; } = nameof(RoleNameAlreadyExists);
    public override string Message { get; set; } = "Já existe uma role com esse nome.";
}

public class InvalidRoleBaseType : SykiError
{
    public static readonly InvalidRoleBaseType I = new();
    public override string Code { get; set; } = nameof(InvalidRoleBaseType);
    public override string Message { get; set; } = "Tipo base do perfil de acesso inválido.";
}

public class InvalidRolePermissions : SykiError
{
    public static readonly InvalidRolePermissions I = new();
    public override string Code { get; set; } = nameof(InvalidRolePermissions);
    public override string Message { get; set; } = "Permissões do perfil de acesso inválidas.";
}

public class InvalidPermissionsForUserType : SykiError
{
    public static readonly InvalidPermissionsForUserType I = new();
    public override string Code { get; set; } = nameof(InvalidPermissionsForUserType);
    public override string Message { get; set; } = "Uma ou mais permissões não são permitidas para o tipo de usuário selecionado.";
}

public class SocialLoginFailed : SykiError
{
    public static readonly SocialLoginFailed I = new();
    public override string Code { get; set; } = nameof(SocialLoginFailed);
    public override string Message { get; set; } = "Falha na autenticação social. Tente novamente.";
}

public class SocialLoginEmailNotVerified : SykiError
{
    public static readonly SocialLoginEmailNotVerified I = new();
    public override string Code { get; set; } = nameof(SocialLoginEmailNotVerified);
    public override string Message { get; set; } = "Email não verificado pelo provedor.";
}

public class SocialLoginSsoRequired : SykiError
{
    public static readonly SocialLoginSsoRequired I = new();
    public override string Code { get; set; } = nameof(SocialLoginSsoRequired);
    public override string Message { get; set; } = "Sua organização requer login via SSO corporativo.";
}
