namespace Syki.Back.Features.Academic.GetNotifications;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetNotificationsController(GetNotificationsService service) : ControllerBase
{
    [HttpGet("academic/notifications")]
    public async Task<IActionResult> Get()
    {
        var notifications = await service.Get(User.InstitutionId());
        
        return Ok(notifications);
    }
}
