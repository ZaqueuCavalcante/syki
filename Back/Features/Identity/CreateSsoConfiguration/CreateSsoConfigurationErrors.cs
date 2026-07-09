namespace Estud.Back.Features.Identity.CreateSsoConfiguration;

public class InvalidSsoAllowedDomains : EstudError
{
    public static readonly InvalidSsoAllowedDomains I = new();
    public override string Code { get; set; } = nameof(InvalidSsoAllowedDomains);
    public override string Message { get; set; } = "Lista de domínios inválida.";
}

public class SsoDomainAlreadyConfigured : EstudError
{
    public static readonly SsoDomainAlreadyConfigured I = new();
    public override string Code { get; set; } = nameof(SsoDomainAlreadyConfigured);
    public override string Message { get; set; } = "Um ou mais domínios já estão configurados em outro SSO.";
}
