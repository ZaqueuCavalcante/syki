namespace Syki.Back.Features.Academic.GetWebhookCall;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetWebhookCallController(GetWebhookCallService service) : ControllerBase
{
    /// <summary>
    /// Chamada de Webhook
    /// </summary>
    /// <remarks>
    /// Retorna os dados da chamada de webhook especificada.
    /// </remarks>
    [HttpGet("academic/webhooks/calls/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId, id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetWebhookCallFullOut>;
internal class ErrorsExamples : ErrorExamplesProvider<WebhookCallNotFound>;
