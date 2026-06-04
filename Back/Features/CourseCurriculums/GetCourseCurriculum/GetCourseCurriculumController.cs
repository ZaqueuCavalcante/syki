namespace Syki.Back.Features.CourseCurriculums.GetCourseCurriculum;

[ApiController, Authorize(Policies.GetCourseCurriculum)]
public class GetCourseCurriculumController(GetCourseCurriculumService service) : ControllerBase
{
    /// <summary>
    /// Grade curricular
    /// </summary>
    /// <remarks>
    /// Retorna os dados de uma grade curricular, incluindo as disciplinas com período, créditos e carga horária.
    /// </remarks>
    [HttpGet("course-curriculums/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCourseCurriculumOut>;
internal class ErrorsExamples : ErrorExamplesProvider<CourseCurriculumNotFound>;
