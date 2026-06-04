namespace Syki.Back.Features.Teachers.AssignCampiToTeacher;

[ApiController, Authorize(Policies.AssignCampiToTeacher)]
public class AssignCampiToTeacherController(AssignCampiToTeacherService service) : ControllerBase
{
    /// <summary>
    /// Vincular campus ao professor
    /// </summary>
    /// <remarks>
    /// Define os campus em que o professor trabalha. Substitui a lista atual.
    /// </remarks>
    [HttpPut("teachers/{id}/assign-campi")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign(int id, [FromBody] AssignCampiToTeacherIn data)
    {
        var result = await service.Assign(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AssignCampiToTeacherIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    TeacherNotFound,
    InvalidCampusList
>;
