using Exato.Shared.Features.Office.CriarUsuario;

namespace Exato.Back.Features.Office.CriarUsuario;

[ApiController, Authorize(Policies.CriarUsuario)]
public class CriarUsuarioController(CriarUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Criar usuário
    /// </summary>
    /// <remarks>
    /// Cria um novo usuário.
    /// </remarks>
    [HttpPost("office/usuarios")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CriarUsuarioIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CriarUsuarioIn>;
internal class ResponseExamples : ExamplesProvider<CriarUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    NomeDeUsuarioInvalido,
    InvalidEmail,
    InvalidCpf,
    EmailAlreadyUsed
>;
