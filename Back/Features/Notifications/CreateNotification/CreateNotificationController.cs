namespace Estud.Back.Features.Notifications.CreateNotification;

[ApiController, Authorize(Policies.CreateNotification)]
public class CreateNotificationController(CreateNotificationService service) : ControllerBase
{
    /// <summary>
    /// Criar notificação
    /// </summary>
    /// <remarks>
    /// Cria uma nova notificação.
    /// </remarks>
    [HttpPost("notifications")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateNotificationIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateNotificationIn>;
internal class ResponseExamples : ExamplesProvider<CreateNotificationOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidNotificationTitle,
    InvalidNotificationDescription
>;
