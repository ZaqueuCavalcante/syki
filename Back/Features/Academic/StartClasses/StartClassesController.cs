namespace Syki.Back.Features.Academic.StartClasses;

/// <summary>
/// Inicia várias Turmas.
/// Após isso, tanto Professores quanto Alunos passam a ter acesso às suas Turmas.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class StartClassesController(StartClassesService service) : ControllerBase
{
    [HttpPut("academic/classes/start")]
    public async Task<IActionResult> Start([FromBody] StartClassesIn data)
    {
        var result = await service.Start(User.InstitutionId(), data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
