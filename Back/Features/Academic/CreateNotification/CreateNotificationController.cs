namespace Syki.Back.Features.Academic.CreateNotification;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateNotificationController(CreateNotificationService service) : ControllerBase
{
    [HttpPost("academic/notifications")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateNotificationIn data)
    {
        var notification = await service.Create(User.InstitutionId(), data);

        return Ok(notification);
    }
}
