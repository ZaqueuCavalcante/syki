namespace Syki.Back.Features.Webhooks.GetWebhookSubscription;

[ApiController, Authorize(Policies.GetWebhookSubscription)]
public class GetWebhookSubscriptionController(GetWebhookSubscriptionService service) : ControllerBase
{
    /// <summary>
    /// Inscrição de webhook
    /// </summary>
    /// <remarks>
    /// Retorna os dados de uma inscrição de webhook.
    /// </remarks>
    [HttpGet("webhooks/subscriptions/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetWebhookSubscriptionOut>;
internal class ErrorsExamples : ErrorExamplesProvider<WebhookSubscriptionNotFound>;
