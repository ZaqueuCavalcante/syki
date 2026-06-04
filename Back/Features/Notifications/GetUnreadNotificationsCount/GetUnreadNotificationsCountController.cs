namespace Syki.Back.Features.Notifications.GetUnreadNotificationsCount;

[ApiController, Authorize(Policies.GetUnreadNotificationsCount)]
public class GetUnreadNotificationsCountController(GetUnreadNotificationsCountService service) : ControllerBase
{
    /// <summary>
    /// Notificações Não Lidas
    /// </summary>
    /// <remarks>
    /// Retorna a quantidade de notificações não lidas do usuário logado.
    /// </remarks>
    [HttpGet("notifications/unread-count")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get();
        return Ok(result);
    }
}

internal class ResponseExamples : ExamplesProvider<GetUnreadNotificationsCountOut>;
