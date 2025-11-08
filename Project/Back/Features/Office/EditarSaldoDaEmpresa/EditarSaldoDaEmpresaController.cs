using Exato.Shared.Features.Office.EditarSaldoDaEmpresa;

namespace Exato.Back.Features.Office.EditarSaldoDaEmpresa;

[ApiController, Authorize(Policies.EditarSaldoDaEmpresa)]
public class EditarSaldoDaEmpresaController(EditarSaldoDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Editar saldo da empresa
    /// </summary>
    /// <remarks>
    /// Adiciona ou remove algum valor no total de saldo/créditos da empresa.
    /// </remarks>
    [HttpPut("office/empresas/{id}/saldo")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarSaldoDaEmpresaIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarSaldoDaEmpresaIn>;
internal class ResponseExamples : ExamplesProvider<EditarSaldoDaEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    EmpresaNaoEncontrada,
    MetodoDePagamentoInvalido,
    ValorEmReaisInvalido,
    ValorEmCreditosInvalido
>;
