namespace Exato.Back.Features.Office.GetCommandBatch;

[ApiController, Authorize(Policies.GetCommandBatch)]
public class GetCommandBatchController(GetCommandBatchService service) : ControllerBase
{
    /// <summary>
    /// Lote
    /// </summary>
    /// <remarks>
    /// Retorna o lote de comandos espeficicado.
    /// </remarks>
    [HttpGet("office/command-batches/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
