using Exato.Shared.Features.Office.EditarTokenDeAcesso;

namespace Exato.Back.Features.Office.EditarTokenDeAcesso;

[ApiController, Authorize(Policies.EditarTokenDeAcessoDaEmpresa)]
public class EditarTokenDeAcessoController(EditarTokenDeAcessoService service) : ControllerBase
{
    /// <summary>
    /// Editar token de acesso
    /// </summary>
    /// <remarks>
    /// Edita o token de acesso informado.
    /// </remarks>
    [HttpPut("office/tokens/{id:int}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarTokenDeAcessoIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarTokenDeAcessoIn>;
internal class ResponseExamples : ExamplesProvider<EditarTokenDeAcessoOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ValidadeDeTokenInvalida,
    NomeDeTokenInvalido,
    DescricaoDeTokenInvalida,
    TokenDeAcessoNaoEncontrado
>;
