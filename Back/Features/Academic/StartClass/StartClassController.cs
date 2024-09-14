namespace Syki.Back.Features.Academic.StartClass;

/// <summary>
/// Inicia várias Turmas.
/// Após isso, tanto Professores quanto Alunos passam a ter acesso às suas Turmas.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class StartClassController(StartClassService service) : ControllerBase
{
    [HttpPut("academic/classes/{id}/start")]
    public async Task<IActionResult> Start([FromRoute] Guid id)
    {
        var result = await service.Start(User.InstitutionId(), id);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
