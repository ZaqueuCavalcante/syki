namespace Syki.Back.Features.Cross.ViewNotifications;

/// <summary>
/// Marca todas as Notificações do Usuário como visualizadas.
/// </summary>
[ApiController, AuthBearer]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class ViewNotificationsController(ViewNotificationsService service) : ControllerBase
{
    [HttpPut("notifications/user")]
    public async Task<IActionResult> View()
    {
        await service.View(User.InstitutionId(), User.Id());

        return Ok();
    }
}
