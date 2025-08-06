namespace Syki.Back.Features.Cross.ViewNotifications;

[ApiController, AuthBearer]
[EnableRateLimiting("Medium")]
public class ViewNotificationsController(ViewNotificationsService service) : ControllerBase
{
    /// <summary>
    /// Visualizar notificações
    /// </summary>
    /// <remarks>
    /// Marca todas as notificações do usuário como visualizadas.
    /// </remarks>
    [HttpPut("notifications/user")]
    public async Task<IActionResult> View()
    {
        await service.View(User.InstitutionId, User.Id);

        return Ok();
    }
}
