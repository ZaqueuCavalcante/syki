namespace Estud.Back.Features.Notifications.GetNotifications;

[ApiController, Authorize(Policies.GetNotifications)]
public class GetNotificationsController(GetNotificationsService service) : ControllerBase
{
    /// <summary>
    /// Listar Notificações
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de notificações do usuário logado, da mais recente para a mais antiga.
    /// </remarks>
    [HttpGet("notifications")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetNotificationsIn data)
    {
        var result = await service.Get(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetNotificationsOut>;
