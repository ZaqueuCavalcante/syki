namespace Syki.Back.Features.Academic.CreateStudent;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateStudentController(CreateStudentService service) : ControllerBase
{
    /// <summary>
    /// Criar aluno
    /// </summary>
    /// <remarks>
    /// Cria um novo aluno.
    /// Um link para redefinição de senha será enviado pro email informado.
    /// </remarks>
    [HttpPost("academic/students")]
    [DbContextTransactionFilter]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateStudentIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateStudentIn>;
internal class ResponseExamples : ExamplesProvider<StudentOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CourseOfferingNotFound,
    InvalidEmail,
    EmailAlreadyUsed,
    WeakPassword>;
