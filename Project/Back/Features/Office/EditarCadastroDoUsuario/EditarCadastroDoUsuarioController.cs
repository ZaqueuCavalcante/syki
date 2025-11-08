using Exato.Shared.Features.Office.EditarCadastroDoUsuario;

namespace Exato.Back.Features.Office.EditarCadastroDoUsuario;

[ApiController, Authorize(Policies.EditarCadastroDoUsuario)]
public class EditarCadastroDoUsuarioController(EditarCadastroDoUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Editar cadastro do usuário
    /// </summary>
    /// <remarks>
    /// Edita os dados de cadastro do usuário.
    /// </remarks>
    [HttpPut("office/usuarios/{id:int}/cadastro")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarCadastroDoUsuarioIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarCadastroDoUsuarioIn>;
internal class ResponseExamples : ExamplesProvider<EditarCadastroDoUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    NomeDeUsuarioInvalido,
    InvalidEmail,
    InvalidCpf,
    UserNotFound,
    EmailAlreadyUsed
>;
