namespace Syki.Back.Features.Cross.GetUserNotifications;

/// <summary>
/// Retorna todas as Notificações vinculadas com o Usuário.
/// </summary>
[ApiController, AuthBearer]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetUserNotificationsController(GetUserNotificationsService service) : ControllerBase
{
    [HttpGet("notifications/user")]
    public async Task<IActionResult> Get()
    {
        var notifications = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(notifications);
    }
}
