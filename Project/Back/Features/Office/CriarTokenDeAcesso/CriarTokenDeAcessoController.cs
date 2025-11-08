using Exato.Shared.Features.Office.CriarTokenDeAcesso;

namespace Exato.Back.Features.Office.CriarTokenDeAcesso;

[ApiController, Authorize(Policies.CriarTokenDeAcessoDaEmpresa)]
public class CriarTokenDeAcessoController(CriarTokenDeAcessoService service) : ControllerBase
{
    /// <summary>
    /// Criar token de acesso
    /// </summary>
    /// <remarks>
    /// Cria um novo token de acesso.
    /// </remarks>
    [HttpPost("office/tokens")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CriarTokenDeAcessoIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CriarTokenDeAcessoIn>;
internal class ResponseExamples : ExamplesProvider<CriarTokenDeAcessoOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    NomeDeTokenInvalido,
    DescricaoDeTokenInvalida,
    EmpresaNaoEncontrada
>;
