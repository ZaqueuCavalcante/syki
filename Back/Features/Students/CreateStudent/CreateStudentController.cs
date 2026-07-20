namespace Estud.Back.Features.Students.CreateStudent;

[ApiController, Authorize(Policies.CreateStudent)]
public class CreateStudentController(CreateStudentService service) : ControllerBase
{
    /// <summary>
    /// Criar aluno
    /// </summary>
    /// <remarks>
    /// Cria um novo aluno.
    /// Um link para redefinição de senha será enviado pro email informado.
    /// </remarks>
    [HttpPost("students")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateStudentIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateStudentIn>;
internal class ResponseExamples : ExamplesProvider<CreateStudentOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidEmail,
    InvalidBirthdate,
    EmailAlreadyUsed,
    InvalidPhoneNumber
>;
