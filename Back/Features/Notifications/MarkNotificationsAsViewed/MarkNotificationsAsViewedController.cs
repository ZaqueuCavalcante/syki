namespace Estud.Back.Features.Notifications.MarkNotificationsAsViewed;

[ApiController, Authorize(Policies.MarkNotificationsAsViewed)]
public class MarkNotificationsAsViewedController(MarkNotificationsAsViewedService service) : ControllerBase
{
    /// <summary>
    /// Marcar Notificações como Lidas
    /// </summary>
    /// <remarks>
    /// Marca uma ou mais notificações como lidas para o usuário logado.
    /// </remarks>
    [HttpPut("notifications/mark-as-viewed")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> MarkAsViewed([FromBody] MarkNotificationsAsViewedIn data)
    {
        var result = await service.MarkAsViewed(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<InvalidNotificationId>;
