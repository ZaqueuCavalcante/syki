namespace Estud.Back.Features.CourseCurriculums.UpdateCourseCurriculum;

[ApiController, Authorize(Policies.UpdateCourseCurriculum)]
public class UpdateCourseCurriculumController(UpdateCourseCurriculumService service) : ControllerBase
{
    /// <summary>
    /// Editar grade curricular
    /// </summary>
    /// <remarks>
    /// Atualiza o nome e as disciplinas de uma grade curricular existente.
    /// </remarks>
    [HttpPut("course-curriculums")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateCourseCurriculumIn data)
    {
        var result = await service.Update(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateCourseCurriculumIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidDisciplinesList,
    CourseCurriculumNotFound,
    InvalidCourseCurriculumName,
    CourseCurriculumWithCourseOffering
>;
