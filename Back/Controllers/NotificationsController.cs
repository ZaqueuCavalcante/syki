namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationsService _service;
    public NotificationsController(INotificationsService service) => _service = service;

    [HttpPost("")]
    [AuthAcademico]
    public async Task<IActionResult> Create([FromBody] NotificationIn data)
    {
        var notification = await _service.Create(User.InstitutionId(), data);

        return Ok(notification);
    }

    [AuthBearer]
    [HttpPut("user")]
    public async Task<IActionResult> ViewByUserId()
    {
        await _service.ViewByUserId(User.InstitutionId(), User.Id());

        return Ok();
    }

    [HttpGet("")]
    [AuthAcademico]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await _service.GetAll(User.InstitutionId());
        
        return Ok(notifications);
    }

    [AuthBearer]
    [HttpGet("user")]
    public async Task<IActionResult> GetByUserId()
    {
        var notifications = await _service.GetByUserId(User.InstitutionId(), User.Id());
        
        return Ok(notifications);
    }
}
