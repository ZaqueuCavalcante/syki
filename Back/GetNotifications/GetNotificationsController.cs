namespace Syki.Back.GetNotifications;

[ApiController]
[EnableRateLimiting("Medium")]
public class GetNotificationsController(GetNotificationsService service) : ControllerBase
{
    [AuthAcademico]
    [HttpGet("notifications")]
    public async Task<IActionResult> Get()
    {
        var notifications = await service.Get(User.InstitutionId());
        
        return Ok(notifications);
    }
}
