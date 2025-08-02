namespace Syki.Back.Features.Adm.GetBatches;

[ApiController, AuthAdm]
public class GetBatchesController(GetBatchesService service) : ControllerBase
{
    /// <summary>
    /// Lotes
    /// </summary>
    /// <remarks>
    /// Retorna todos os lotes.
    /// </remarks>
    [HttpGet("adm/batches")]
    public async Task<IActionResult> Get([FromQuery] CommandBatchTableFilterIn filters)
    {
        var batches = await service.Get(filters);

        return Ok(batches);
    }
}
