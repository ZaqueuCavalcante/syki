namespace Syki.Back.Features.Webhooks.GetWebhookCalls;

[ApiController, Authorize(Policies.GetWebhookCalls)]
public class GetWebhookCallsController(GetWebhookCallsService service) : ControllerBase
{
    /// <summary>
    /// Chamadas de webhook
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de chamadas de webhook da instituição, da mais recente para a mais antiga.
    /// </remarks>
    [HttpGet("webhooks/calls")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetWebhookCallsIn data)
    {
        var calls = await service.Get(data);
        return Ok(calls);
    }
}

internal class ResponseExamples : ExamplesProvider<GetWebhookCallsOut>;
