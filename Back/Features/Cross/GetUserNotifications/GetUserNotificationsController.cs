namespace Syki.Back.Features.Cross.GetUserNotifications;

[ApiController, AuthBearer]
[EnableRateLimiting("Medium")]
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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var notifications = await service.Get(User.InstitutionId, User.Id);
        return Ok(notifications);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<List<UserNotificationOut>>
{
    public IEnumerable<SwaggerExample<List<UserNotificationOut>>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Nofitications",
			new List<UserNotificationOut>()
			{
				new()
				{
					NotificationId = Guid.CreateVersion7(),
					Title = "Boas-vindas!",
					Description = "Agradecemos a confiança na nossa instituição, que seja uma jornada cheia de aprendizado!",
					CreatedAt = DateTime.UtcNow.AddDays(-50),
					ViewedAt = DateTime.UtcNow.AddDays(-48),
				},
				new()
				{
					NotificationId = Guid.CreateVersion7(),
					Title = "Semana de prova chegando!",
					Description = "Preparado(a) pras avaliações? Elas começam semana que vem, revise os assuntos e boas provas!",
					CreatedAt = DateTime.UtcNow,
					ViewedAt = null,
				}
			}
		);
    }
}
