using Exato.Shared.Features.Office.CreateUser;

namespace Exato.Back.Features.Office.CreateUser;

[ApiController, Authorize(Policies.CreateUser)]
public class CreateUserController(CreateUserService service) : ControllerBase
{
    /// <summary>
    /// Criar usuário
    /// </summary>
    /// <remarks>
    /// Cria um novo usuário para a organização informada.
    /// </remarks>
    [HttpPost("office/users")]
    [DbContextTransactionFilter]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateUserIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateUserIn>;
internal class ResponseExamples : ExamplesProvider<CreateUserOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    EmpresaNaoEncontrada,
    InvalidEmail,
    EmailAlreadyUsed,
    RoleNotFound,
    WeakPassword
>;
