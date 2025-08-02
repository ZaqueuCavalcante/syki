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
    [HttpGet("academic/webhooks/calls/{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var webhookCall = await service.Get(User.InstitutionId(), id);

        return Ok(webhookCall);
    }
}
