namespace Syki.Back.Controllers;

[ApiController]
[EnableRateLimiting("Medium")]
public class NotificationsController(INotificationsService service) : ControllerBase
{
    [HttpPost("notifications")]
    [AuthAcademico]
    public async Task<IActionResult> Create([FromBody] NotificationIn data)
    {
        var notification = await service.Create(User.InstitutionId(), data);

        return Ok(notification);
    }

    [AuthBearer]
    [HttpPut("notifications/user")]
    public async Task<IActionResult> ViewByUserId()
    {
        await service.ViewByUserId(User.InstitutionId(), User.Id());

        return Ok();
    }

    [HttpGet("notifications")]
    [AuthAcademico]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await service.GetAll(User.InstitutionId());
        
        return Ok(notifications);
    }

    [AuthBearer]
    [HttpGet("notifications/user")]
    public async Task<IActionResult> GetByUserId()
    {
        var notifications = await service.GetByUserId(User.InstitutionId(), User.Id());
        
        return Ok(notifications);
    }
}
