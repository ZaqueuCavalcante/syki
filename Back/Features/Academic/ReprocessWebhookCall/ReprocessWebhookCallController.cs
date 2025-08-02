namespace Syki.Back.Features.Academic.ReprocessWebhookCall;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class ReprocessWebhookCallController(ReprocessWebhookCallService service) : ControllerBase
{
    /// <summary>
    /// Reprocessar Webhook
    /// </summary>
    /// <remarks>
    /// Reprocessa uma chamada de Webhook.
    /// </remarks>
    [HttpPost("academic/webhooks/calls/{id:guid}/reprocess")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Reprocess([FromRoute] Guid id)
    {
        var result = await service.Reprocess(User.InstitutionId(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
