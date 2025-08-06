namespace Syki.Back.Features.Academic.GetWebhook;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetWebhookController(GetWebhookService service) : ControllerBase
{
    /// <summary>
    /// Webhook
    /// </summary>
    /// <remarks>
    /// Retorna os dados do webhook especificado.
    /// </remarks>
    [HttpGet("academic/webhooks/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId, id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetWebhookOut>;
internal class ErrorsExamples : ErrorExamplesProvider<WebhookNotFound>;
