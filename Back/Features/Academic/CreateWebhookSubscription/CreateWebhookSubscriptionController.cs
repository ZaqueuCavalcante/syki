namespace Syki.Back.Features.Academic.CreateWebhookSubscription;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateWebhookSubscriptionController(CreateWebhookSubscriptionService service) : ControllerBase
{
    /// <summary>
    /// Criar Webhook
    /// </summary>
    /// <remarks>
    /// Cria uma inscrição de Webhook.
    /// </remarks>
    [HttpPost("academic/webhooks")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateWebhookSubscriptionIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateWebhookSubscriptionIn>
{
    public IEnumerable<SwaggerExample<CreateWebhookSubscriptionIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Aluno criado",
            new CreateWebhookSubscriptionIn
            {
                Name = "Aluno criado",
                Url = "https://webhook.site/my-webhook",
                AuthenticationType = WebhookAuthenticationType.ApiKey,
                ApiKey = "z3Q6uDUJYTDCIo16myBKZrlCS63IvpCUOAE5X",
                Events = [WebhookEventType.StudentCreated]
            }
        );
        yield return SwaggerExample.Create(
			"Atividade publicada",
			new CreateWebhookSubscriptionIn
            {
                Name = "Atividade publicada",
                Url = "https://webhook.site/my-other-webhook",
                AuthenticationType = WebhookAuthenticationType.ApiKey,
                ApiKey = "z3Q6uDUJYTDCIo16myBKZrlCS63IvpCUOAE5X",
                Events = [WebhookEventType.ClassActivityCreated]
			}
		);
    }
}
