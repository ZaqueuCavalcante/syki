namespace Syki.Back.Features.Teacher.CreateClassActivity;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class CreateClassActivityController(CreateClassActivityService service) : ControllerBase
{
    /// <summary>
    /// Criar atividade
    /// </summary>
    /// <remarks>
    /// Cria uma atividade vinculada à turma especificada
    /// </remarks>
    [HttpPost("teacher/classes/{id}/activities")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateClassActivityIn data)
    {
        var result = await service.Create(User.Id, id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateClassActivityIn>;
internal class ResponseExamples : ExamplesProvider<CreateClassActivityOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassNotFound,
    InvalidClassActivityWeight>;
