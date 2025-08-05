namespace Syki.Back.Features.Academic.CreateNotification;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateNotificationController(CreateNotificationService service) : ControllerBase
{
    /// <summary>
    /// Criar notificação
    /// </summary>
    /// <remarks>
    /// Cria uma nova notificação.
    /// </remarks>
    [HttpPost("academic/notifications")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Create([FromBody] CreateNotificationIn data)
    {
        var notification = await service.Create(User.InstitutionId(), data);
        return Ok(notification);
    }
}

internal class RequestExamples : ExamplesProvider<CreateNotificationIn>;
internal class ResponseExamples : ExamplesProvider<NotificationOut>;
