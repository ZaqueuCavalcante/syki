namespace Syki.Back.Features.Adm.GetBatch;

[ApiController, AuthAdm]
public class GetBatchController(GetBatchService service) : ControllerBase
{
    /// <summary>
    /// Lote
    /// </summary>
    /// <remarks>
    /// Retorna o lote especificado.
    /// </remarks>
    [HttpGet("adm/batches/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var batch = await service.Get(id);

        return Ok(batch);
    }
}
