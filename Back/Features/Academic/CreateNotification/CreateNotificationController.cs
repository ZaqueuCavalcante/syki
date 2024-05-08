namespace Syki.Back.CreateNotification;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateNotificationController(CreateNotificationService service) : ControllerBase
{
    [HttpPost("notifications")]
    public async Task<IActionResult> Create([FromBody] NotificationIn data)
    {
        var notification = await service.Create(User.InstitutionId(), data);

        return Ok(notification);
    }
}
