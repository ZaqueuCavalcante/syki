namespace Syki.Back.Features.Academic.GetAcademicTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetAcademicTeacherController(GetAcademicTeacherService service) : ControllerBase
{
    /// <summary>
    /// Professor
    /// </summary>
    /// <remarks>
    /// Retorna os dados do professor especificado.
    /// </remarks>
    [HttpGet("academic/teachers/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId, id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetAcademicTeacherOut>;
internal class ErrorsExamples : ErrorExamplesProvider<TeacherNotFound>;
