namespace Syki.Back.Features.Courses.GetCoursePotentialDisciplines;

[ApiController, Authorize(Policies.GetCoursePotentialDisciplines)]
public class GetCoursePotentialDisciplinesController(GetCoursePotentialDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas disponíveis para vincular ao curso
    /// </summary>
    /// <remarks>
    /// Retorna as disciplinas ainda não vinculadas ao curso, com suporte a pesquisa por nome.
    /// </remarks>
    [HttpGet("courses/{id}/potential-disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id, [FromQuery] string? name)
    {
        var result = await service.Get(id, name);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCoursePotentialDisciplinesOut>;
internal class ErrorsExamples : ErrorExamplesProvider<CourseNotFound>;
