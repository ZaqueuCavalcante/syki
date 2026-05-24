namespace Syki.Back.Features.Disciplines.GetDisciplinePotentialCourses;

[ApiController, Authorize(Policies.GetDisciplinePotentialCourses)]
public class GetDisciplinePotentialCoursesController(GetDisciplinePotentialCoursesService service) : ControllerBase
{
    /// <summary>
    /// Cursos disponíveis para vincular à disciplina
    /// </summary>
    /// <remarks>
    /// Retorna os cursos ainda não vinculados à disciplina, com suporte a pesquisa por nome.
    /// </remarks>
    [HttpGet("disciplines/{id}/potential-courses")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id, [FromQuery] string? name)
    {
        var result = await service.Get(id, name);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetDisciplinePotentialCoursesOut>;
internal class ErrorsExamples : ErrorExamplesProvider<DisciplineNotFound>;
