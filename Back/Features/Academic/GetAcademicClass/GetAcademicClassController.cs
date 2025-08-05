namespace Syki.Back.Features.Academic.GetAcademicClass;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetAcademicClassController(GetAcademicClassService service) : ControllerBase
{
    /// <summary>
    /// Turma
    /// </summary>
    /// <remarks>
    /// Retorna a turma informada.
    /// </remarks>
    [HttpGet("academic/classes/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId(), id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetAcademicClassOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound>;
