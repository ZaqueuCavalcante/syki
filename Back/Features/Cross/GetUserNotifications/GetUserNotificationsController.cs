namespace Syki.Back.Features.Cross.GetUserNotifications;

[ApiController, AuthBearer]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetUserNotificationsController(GetUserNotificationsService service) : ControllerBase
{
    /// <summary>
    /// Notificações
    /// </summary>
    /// <remarks>
    /// Retorna as notificações do usuário.
    /// </remarks>
    [HttpGet("notifications/user")]
    [ProducesResponseType(typeof(List<UserNotificationOut>), 200)]
    [SwaggerResponseExample(200, typeof(GetUserNotificationsResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var notifications = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(notifications);
    }
}
