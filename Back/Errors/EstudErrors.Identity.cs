namespace Estud.Back.Errors;

public class InvalidTwoFactorToken : EstudError
{
    public static readonly InvalidTwoFactorToken I = new();
    public override string Code { get; set; } = nameof(InvalidTwoFactorToken);
    public override string Message { get; set; } = "2FA token inválido.";
}

public class InvalidRoleName : EstudError
{
    public static readonly InvalidRoleName I = new();
    public override string Code { get; set; } = nameof(InvalidRoleName);
    public override string Message { get; set; } = "Nome de role inválido.";
}

public class InvalidRoleDescription : EstudError
{
    public static readonly InvalidRoleDescription I = new();
    public override string Code { get; set; } = nameof(InvalidRoleDescription);
    public override string Message { get; set; } = "Descrição de role inválida.";
}

public class InvalidPermissionsList : EstudError
{
    public static readonly InvalidPermissionsList I = new();
    public override string Code { get; set; } = nameof(InvalidPermissionsList);
    public override string Message { get; set; } = "Lista de permissions inválida.";
}

public class InvalidSsoProviderType : EstudError
{
    public static readonly InvalidSsoProviderType I = new();
    public override string Code { get; set; } = nameof(InvalidSsoProviderType);
    public override string Message { get; set; } = "Tipo de provedor SSO inválido.";
}

public class InvalidSsoAuthority : EstudError
{
    public static readonly InvalidSsoAuthority I = new();
    public override string Code { get; set; } = nameof(InvalidSsoAuthority);
    public override string Message { get; set; } = "URL do provedor SSO inválida.";
}

public class InvalidSsoClientId : EstudError
{
    public static readonly InvalidSsoClientId I = new();
    public override string Code { get; set; } = nameof(InvalidSsoClientId);
    public override string Message { get; set; } = "Client ID inválido.";
}

public class InvalidSsoClientSecret : EstudError
{
    public static readonly InvalidSsoClientSecret I = new();
    public override string Code { get; set; } = nameof(InvalidSsoClientSecret);
    public override string Message { get; set; } = "Client Secret inválido.";
}

public class SsoAuthorityMustBeHttps : EstudError
{
    public static readonly SsoAuthorityMustBeHttps I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityMustBeHttps);
    public override string Message { get; set; } = "URL do provedor SSO deve usar HTTPS.";
}

public class SsoAuthorityLocalhostNotAllowed : EstudError
{
    public static readonly SsoAuthorityLocalhostNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityLocalhostNotAllowed);
    public override string Message { get; set; } = "Localhost não é permitido em produção.";
}

public class SsoAuthorityPrivateIpNotAllowed : EstudError
{
    public static readonly SsoAuthorityPrivateIpNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityPrivateIpNotAllowed);
    public override string Message { get; set; } = "IP privado não é permitido em produção.";
}

public class SsoAuthorityHasUserInfo : EstudError
{
    public static readonly SsoAuthorityHasUserInfo I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityHasUserInfo);
    public override string Message { get; set; } = "URL do provedor SSO não pode conter credenciais.";
}

public class SsoAuthorityLinkLocalNotAllowed : EstudError
{
    public static readonly SsoAuthorityLinkLocalNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityLinkLocalNotAllowed);
    public override string Message { get; set; } = "IP link-local não é permitido.";
}

public class SsoAuthorityLoopbackNotAllowed : EstudError
{
    public static readonly SsoAuthorityLoopbackNotAllowed I = new();
    public override string Code { get; set; } = nameof(SsoAuthorityLoopbackNotAllowed);
    public override string Message { get; set; } = "IP de loopback não é permitido em produção.";
}

public class SsoConfigurationNotFound : EstudError
{
    public static readonly SsoConfigurationNotFound I = new();
    public override string Code { get; set; } = nameof(SsoConfigurationNotFound);
    public override string Message { get; set; } = "Configuração SSO não encontrada.";
}

public class SsoNotConfiguredForDomain : EstudError
{
    public static readonly SsoNotConfiguredForDomain I = new();
    public override string Code { get; set; } = nameof(SsoNotConfiguredForDomain);
    public override string Message { get; set; } = "SSO não está configurado para este domínio.";
}

public class SsoAuthenticationFailed : EstudError
{
    public static readonly SsoAuthenticationFailed I = new();
    public override string Code { get; set; } = nameof(SsoAuthenticationFailed);
    public override string Message { get; set; } = "Falha na autenticação SSO.";
}

public class SsoLoginUserNotFound : EstudError
{
    public static readonly SsoLoginUserNotFound I = new();
    public override string Code { get; set; } = nameof(SsoLoginUserNotFound);
    public override string Message { get; set; } = "Usuário não pertence à organização.";
}

public class RoleNotFound : EstudError
{
    public static readonly RoleNotFound I = new();
    public override string Code { get; set; } = nameof(RoleNotFound);
    public override string Message { get; set; } = "Role não encontrada.";
}

public class RoleNameAlreadyExists : EstudError
{
    public static readonly RoleNameAlreadyExists I = new();
    public override string Code { get; set; } = nameof(RoleNameAlreadyExists);
    public override string Message { get; set; } = "Já existe uma role com esse nome.";
}

public class InvalidRoleBaseType : EstudError
{
    public static readonly InvalidRoleBaseType I = new();
    public override string Code { get; set; } = nameof(InvalidRoleBaseType);
    public override string Message { get; set; } = "Tipo base do perfil de acesso inválido.";
}

public class InvalidRolePermissions : EstudError
{
    public static readonly InvalidRolePermissions I = new();
    public override string Code { get; set; } = nameof(InvalidRolePermissions);
    public override string Message { get; set; } = "Permissões do perfil de acesso inválidas.";
}

public class InvalidPermissionsForUserType : EstudError
{
    public static readonly InvalidPermissionsForUserType I = new();
    public override string Code { get; set; } = nameof(InvalidPermissionsForUserType);
    public override string Message { get; set; } = "Uma ou mais permissões não são permitidas para o tipo de usuário selecionado.";
}

public class SocialLoginFailed : EstudError
{
    public static readonly SocialLoginFailed I = new();
    public override string Code { get; set; } = nameof(SocialLoginFailed);
    public override string Message { get; set; } = "Falha na autenticação social. Tente novamente.";
}

public class SocialLoginEmailNotVerified : EstudError
{
    public static readonly SocialLoginEmailNotVerified I = new();
    public override string Code { get; set; } = nameof(SocialLoginEmailNotVerified);
    public override string Message { get; set; } = "Email não verificado pelo provedor.";
}

public class SocialLoginSsoRequired : EstudError
{
    public static readonly SocialLoginSsoRequired I = new();
    public override string Code { get; set; } = nameof(SocialLoginSsoRequired);
    public override string Message { get; set; } = "Sua organização requer login via SSO corporativo.";
}
