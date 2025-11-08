namespace Exato.Back.Features.Office.ReprocessCommand;

[ApiController, Authorize(Policies.ReprocessCommand)]
public class ReprocessCommandController(ReprocessCommandService service) : ControllerBase
{
    /// <summary>
    /// Comando
    /// </summary>
    /// <remarks>
    /// Reprocessa o comando especificado.
    /// </remarks>
    [HttpPost("office/commands/{id}/reprocess")]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Reprocess([FromRoute] Guid id)
    {
        var result = await service.Reprocess(id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ErrorsExamples : ErrorExamplesProvider<
    CommandNotFound,
    OnlyRootCommandsCanBeReprocessed
>;
