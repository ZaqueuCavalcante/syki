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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateWebhookSubscriptionIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateWebhookSubscriptionIn>;
internal class ResponseExamples : ExamplesProvider<CreateWebhookSubscriptionOut>;
internal class ErrorsExamples : ErrorExamplesProvider<InvalidWebhookAuthentication>;
