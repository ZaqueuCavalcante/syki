namespace Estud.Back.Features.CourseCurriculums.EditCourseCurriculum;

[ApiController, Authorize(Policies.EditCourseCurriculum)]
public class EditCourseCurriculumController(EditCourseCurriculumService service) : ControllerBase
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
    public async Task<IActionResult> Edit([FromBody] EditCourseCurriculumIn data)
    {
        var result = await service.Edit(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditCourseCurriculumIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CourseCurriculumNotFound,
    InvalidCourseCurriculumName,
    InvalidDisciplinesList
>;
