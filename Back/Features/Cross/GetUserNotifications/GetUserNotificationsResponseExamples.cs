namespace Syki.Back.Features.Cross.GetUserNotifications;

public class GetUserNotificationsResponseExamples : IMultipleExamplesProvider<List<UserNotificationOut>>
{
    public IEnumerable<SwaggerExample<List<UserNotificationOut>>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Nofitications",
			new List<UserNotificationOut>()
			{
				new()
				{
					NotificationId = Guid.NewGuid(),
					Title = "Boas-vindas!",
					Description = "Agradecemos a confiança na nossa instituição, que seja uma jornada com muito aprendizado pra você!",
					CreatedAt = DateTime.Now.AddDays(-50),
					ViewedAt = DateTime.Now.AddDays(-48),
				},
				new()
				{
					NotificationId = Guid.NewGuid(),
					Title = "Semana de prova chegando!",
					Description = "Preparado pras provas? Elas começam semana que vem, revise os assuntos e boas provas!",
					CreatedAt = DateTime.Now,
					ViewedAt = null,
				}
			}
		);
    }
}
