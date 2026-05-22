namespace Syki.Back.Features.Identity.CreateSsoConfiguration;

[ApiController, Authorize(Policies.CreateSsoConfiguration)]
public class CreateSsoConfigurationController(CreateSsoConfigurationService service) : ControllerBase
{
    /// <summary>
    /// Criar configuração SSO
    /// </summary>
    /// <remarks>
    /// Cria uma nova configuração de Single Sign-On (SSO) para a instituição.
    /// </remarks>
    [HttpPost("identity/sso/configurations")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateSsoConfigurationIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateSsoConfigurationIn>;
internal class ResponseExamples : ExamplesProvider<CreateSsoConfigurationOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidSsoProviderType,
    InvalidSsoAuthority,
    InvalidSsoClientId,
    InvalidSsoClientSecret,
    SsoAuthorityMustBeHttps,
    SsoAuthorityHasUserInfo,
    SsoAuthorityLocalhostNotAllowed,
    SsoAuthorityLoopbackNotAllowed,
    SsoAuthorityPrivateIpNotAllowed,
    SsoAuthorityLinkLocalNotAllowed,
    SsoDomainAlreadyConfigured
>;
