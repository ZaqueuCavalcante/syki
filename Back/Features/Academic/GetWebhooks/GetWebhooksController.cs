namespace Syki.Back.Features.Academic.GetWebhooks;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetWebhooksController(GetWebhooksService service) : ControllerBase
{
    /// <summary>
    /// Webhooks
    /// </summary>
    /// <remarks>
    /// Retorna todos os webhooks.
    /// </remarks>
    [HttpGet("academic/webhooks")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var webhooks = await service.Get(User.InstitutionId());

        return Ok(webhooks);
    }
}
