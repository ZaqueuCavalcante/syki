namespace Syki.Back.Features.Disciplines.GetDiscipline;

[ApiController, Authorize(Policies.GetDiscipline)]
public class GetDisciplineController(GetDisciplineService service) : ControllerBase
{
    /// <summary>
    /// Disciplina
    /// </summary>
    /// <remarks>
    /// Retorna os dados de uma disciplina, incluindo os cursos vinculados.
    /// </remarks>
    [HttpGet("disciplines/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetDisciplineOut>;
internal class ErrorsExamples : ErrorExamplesProvider<DisciplineNotFound>;
