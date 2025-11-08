using Exato.Shared.Features.Office.GetCommandBatches;

namespace Exato.Back.Features.Office.GetCommandBatches;

[ApiController, Authorize(Policies.GetCommandBatches)]
public class GetCommandBatchesController(GetCommandBatchesService service) : ControllerBase
{
    /// <summary>
    /// Lotes
    /// </summary>
    /// <remarks>
    /// Retorna os lotes de comandos.
    /// </remarks>
    [HttpGet("office/command-batches")]
    public async Task<IActionResult> Get([FromQuery] GetCommandBatchesIn data)
    {
        var batches = await service.Get(data);
        return Ok(batches);
    }
}
