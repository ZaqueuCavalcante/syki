namespace Syki.Back.Features.Cross.ViewNotification;

[ApiController, AuthBearer]
[EnableRateLimiting("Medium")]
public class ViewNotificationController(ViewNotificationService service) : ControllerBase
{
    [HttpPut("notifications/user")]
    public async Task<IActionResult> View()
    {
        await service.View(User.InstitutionId(), User.Id());

        return Ok();
    }
}
