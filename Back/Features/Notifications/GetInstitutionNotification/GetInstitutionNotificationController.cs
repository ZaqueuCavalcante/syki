namespace Estud.Back.Features.Notifications.GetInstitutionNotification;

[ApiController, Authorize(Policies.GetInstitutionNotification)]
public class GetInstitutionNotificationController(GetInstitutionNotificationService service) : ControllerBase
{
    /// <summary>
    /// Notificação
    /// </summary>
    /// <remarks>
    /// Retorna os dados de uma notificação da instituição, incluindo a quantidade de visualizações agrupadas por dia.
    /// </remarks>
    [HttpGet("notifications/institution/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetInstitutionNotificationOut>;
internal class ErrorsExamples : ErrorExamplesProvider<NotificationNotFound>;
