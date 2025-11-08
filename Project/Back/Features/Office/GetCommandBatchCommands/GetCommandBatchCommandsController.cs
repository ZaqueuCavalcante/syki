using Exato.Shared.Features.Office.GetCommandBatchCommands;

namespace Exato.Back.Features.Office.GetCommandBatchCommands;

[ApiController, Authorize(Policies.GetCommandBatchCommands)]
public class GetCommandBatchCommandsController(GetCommandBatchCommandsService service) : ControllerBase
{
    /// <summary>
    /// Comandos do lote
    /// </summary>
    /// <remarks>
    /// Retorna os comandos do lote informado.
    /// </remarks>
    [HttpGet("office/command-batches/{id}/commands")]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromQuery] GetCommandBatchCommandsIn data)
    {
        var commands = await service.Get(id, data);
        return Ok(commands);
    }
}
