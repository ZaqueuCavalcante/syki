namespace Syki.Back.Features.Cross.GetHomeStats;

[ApiController, Authorize(Policies.GetHomeStats)]
public class GetHomeStatsController(GetHomeStatsService service) : ControllerBase
{
    /// <summary>
    /// Estatísticas da home
    /// </summary>
    /// <remarks>
    /// Retorna os totais de alunos matriculados, professores ativos, cursos ofertados e disciplinas cadastradas.
    /// </remarks>
    [HttpGet("home/stats")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var stats = await service.Get();
        return Ok(stats);
    }
}

internal class ResponseExamples : ExamplesProvider<GetHomeStatsOut>;
