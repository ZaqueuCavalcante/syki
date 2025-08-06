namespace Syki.Back.Features.Academic.AddDisciplinePreRequisites;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class AddDisciplinePreRequisitesController(AddDisciplinePreRequisitesService service) : ControllerBase
{
    /// <summary>
    /// Adicionar pré-requisitos
    /// </summary>
    /// <remarks>
    /// Adiciona pré-requisitos à uma disciplina, dentro de uma grade curricular.
    /// </remarks>
    [HttpPost("academic/course-curriculums/{id}/pre-requisites")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] AddDisciplinePreRequisitesIn data)
    {
        var result = await service.Add(User.InstitutionId, id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AddDisciplinePreRequisitesIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CourseCurriculumNotFound,
    DisciplineNotFound,
    InvalidDisciplinesList>;
