namespace Estud.Back.Features.Parents.CreateParent;

[ApiController, Authorize(Policies.CreateParent)]
public class CreateParentController(CreateParentService service) : ControllerBase
{
    /// <summary>
    /// Criar responsável
    /// </summary>
    /// <remarks>
    /// Cria um novo responsável vinculado a um ou mais alunos da instituição.
    /// </remarks>
    [HttpPost("parents")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateParentIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateParentIn>;
internal class ResponseExamples : ExamplesProvider<CreateParentOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidEmail,
    EmailAlreadyUsed,
    InvalidPhoneNumber,
    StudentNotFound,
    InvalidParentStudentsList,
    InvalidParentRelationship
>;
