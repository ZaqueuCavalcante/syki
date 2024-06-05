namespace Syki.Back.Features.Academic.StartClass;

/// <summary>
/// Inicia uma turma.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class StartClassController(StartClassService service) : ControllerBase
{
    [HttpPut("academic/classes/start")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Start([FromBody] StartClassIn data)
    {
        await service.Start(User.InstitutionId(), data);

        return NoContent();
    }
}
