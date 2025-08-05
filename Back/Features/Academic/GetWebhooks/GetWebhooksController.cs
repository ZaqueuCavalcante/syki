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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var webhooks = await service.Get(User.InstitutionId());
        return Ok(webhooks);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<List<GetWebhooksOut>>
{
    public IEnumerable<SwaggerExample<List<GetWebhooksOut>>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Webhooks",
            new List<GetWebhooksOut>
            {
                new() {
                    Id = Guid.CreateVersion7(),
                    Name = "GitHub",
                    Url = "https://github.com/my-webhook",
                    CallsCount = 42,
                    CreatedAt = DateTime.Now.AddDays(-7)
                }
            }
		);
    }
}
