namespace Syki.Back.GetNotifications;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetNotificationsController(GetNotificationsService service) : ControllerBase
{
    [HttpGet("notifications")]
    public async Task<IActionResult> Get()
    {
        var notifications = await service.Get(User.InstitutionId());
        
        return Ok(notifications);
    }
}
