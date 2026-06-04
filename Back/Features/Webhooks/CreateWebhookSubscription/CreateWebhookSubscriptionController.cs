namespace Syki.Back.Features.Webhooks.CreateWebhookSubscription;

[ApiController, Authorize(Policies.CreateWebhookSubscription)]
public class CreateWebhookSubscriptionController(CreateWebhookSubscriptionService service) : ControllerBase
{
    /// <summary>
    /// Criar inscrição de webhook
    /// </summary>
    /// <remarks>
    /// Cria uma nova inscrição de webhook vinculada à instituição do usuário logado.
    /// </remarks>
    [HttpPost("webhooks/subscriptions")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateWebhookSubscriptionIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateWebhookSubscriptionIn>;
internal class ResponseExamples : ExamplesProvider<CreateWebhookSubscriptionOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidWebhookName,
    InvalidWebhookUrl,
    InvalidWebhookEvents
>;
