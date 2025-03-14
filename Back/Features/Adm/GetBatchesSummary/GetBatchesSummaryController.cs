namespace Syki.Back.Features.Adm.GetBatchesSummary;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetBatchesSummaryController(GetBatchesSummaryService service) : ControllerBase
{
    /// <summary>
    /// Sumário de Lotes
    /// </summary>
    /// <remarks>
    /// Retorna dados consolidados sobre os lotes.
    /// </remarks>
    [HttpGet("adm/batches/summary")]
    public async Task<IActionResult> Get()
    {
        var batches = await service.Get();

        return Ok(batches);
    }
}
