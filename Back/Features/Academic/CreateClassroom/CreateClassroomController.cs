namespace Syki.Back.Features.Academic.CreateClassroom;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateClassroomController(CreateClassroomService service) : ControllerBase
{
    /// <summary>
    /// Criar sala de aula
    /// </summary>
    /// <remarks>
    /// Cria uma nova sala de aula.
    /// </remarks>
    [HttpPost("academic/classrooms")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateClassroomIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateClassroomIn>;
internal class ResponseExamples : ExamplesProvider<CreateClassroomOut>;
internal class ErrorsExamples : ErrorExamplesProvider<CampusNotFound>;
