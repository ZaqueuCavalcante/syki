namespace Syki.Back.Features.Academic.AssignCampiToTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class AssignCampiToTeacherController(AssignCampiToTeacherService service) : ControllerBase
{
    /// <summary>
    /// Vincular campi
    /// </summary>
    /// <remarks>
    /// Vincula os campus que o professor trabalha
    /// </remarks>
    [HttpPut("academic/teachers/{id}/assign-campi")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign([FromRoute] Guid id, [FromBody] AssignCampiToTeacherIn data)
    {
        var result = await service.Assign(User.InstitutionId, id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AssignCampiToTeacherIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    TeacherNotFound,
    InvalidCampusList>;
