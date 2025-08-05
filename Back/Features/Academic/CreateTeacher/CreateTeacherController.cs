namespace Syki.Back.Features.Academic.CreateTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateTeacherController(CreateTeacherService service) : ControllerBase
{
    /// <summary>
    /// Criar professor
    /// </summary>
    /// <remarks>
    /// Cria um novo professor.
    /// Um link para redefinição de senha será enviado pro email informado.
    /// </remarks>
    [HttpPost("academic/teachers")]
    [DbContextTransactionFilter]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateTeacherIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateTeacherIn>;
internal class ResponseExamples : ExamplesProvider<TeacherOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidEmail,
    EmailAlreadyUsed,
    WeakPassword>;
