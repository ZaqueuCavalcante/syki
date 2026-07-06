namespace Syki.Back.Features.Webhooks.UpdateWebhookSubscription;

[ApiController, Authorize(Policies.UpdateWebhookSubscription)]
public class UpdateWebhookSubscriptionController(UpdateWebhookSubscriptionService service) : ControllerBase
{
    /// <summary>
    /// Editar inscrição de webhook
    /// </summary>
    /// <remarks>
    /// Atualiza os dados de uma inscrição de webhook.
    /// </remarks>
    [HttpPut("webhooks/subscriptions")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateWebhookSubscriptionIn data)
    {
        var result = await service.Update(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateWebhookSubscriptionIn>;
internal class ResponseExamples : ExamplesProvider<UpdateWebhookSubscriptionOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidWebhookName,
    InvalidWebhookUrl,
    InvalidWebhookEvents,
    InvalidWebhookCustomHeaders,
    WebhookSubscriptionNotFound
>;
