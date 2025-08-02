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
    [HttpGet("academic/webhooks/{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var webhook = await service.Get(User.InstitutionId(), id);

        return Ok(webhook);
    }
}
