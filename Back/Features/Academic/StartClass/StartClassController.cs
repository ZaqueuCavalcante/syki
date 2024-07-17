namespace Syki.Back.Features.Academic.StartClass;

/// <summary>
/// Inicia uma turma.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class StartClassController(StartClassService service) : ControllerBase
{
    [HttpPut("academic/classes/{id}/start")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Start([FromRoute] Guid id)
    {
        await service.Start(User.InstitutionId(), id);

        return NoContent();
    }
}
