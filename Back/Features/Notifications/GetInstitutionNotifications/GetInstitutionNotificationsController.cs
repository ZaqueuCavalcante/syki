namespace Estud.Back.Features.Notifications.GetInstitutionNotifications;

[ApiController, Authorize(Policies.GetInstitutionNotifications)]
public class GetInstitutionNotificationsController(GetInstitutionNotificationsService service) : ControllerBase
{
    /// <summary>
    /// Listar Notificações da Instituição
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de notificações personalizadas da instituição, da mais recente para a mais antiga.
    /// </remarks>
    [HttpGet("notifications/institution")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetInstitutionNotificationsIn data)
    {
        var result = await service.Get(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<GetInstitutionNotificationsIn>;
internal class ResponseExamples : ExamplesProvider<GetInstitutionNotificationsOut>;
