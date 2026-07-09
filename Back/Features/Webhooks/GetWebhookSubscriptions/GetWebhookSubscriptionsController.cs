namespace Estud.Back.Features.Webhooks.GetWebhookSubscriptions;

[ApiController, Authorize(Policies.GetWebhookSubscriptions)]
public class GetWebhookSubscriptionsController(GetWebhookSubscriptionsService service) : ControllerBase
{
    /// <summary>
    /// Inscrições de webhook
    /// </summary>
    /// <remarks>
    /// Retorna todas as inscrições de webhook da instituição.
    /// </remarks>
    [HttpGet("webhooks/subscriptions")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var subscriptions = await service.Get();
        return Ok(subscriptions);
    }
}

internal class ResponseExamples : ExamplesProvider<GetWebhookSubscriptionsOut>;
