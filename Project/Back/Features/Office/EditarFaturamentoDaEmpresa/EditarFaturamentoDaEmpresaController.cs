using Exato.Shared.Features.Office.EditarFaturamentoDaEmpresa;

namespace Exato.Back.Features.Office.EditarFaturamentoDaEmpresa;

[ApiController, Authorize(Policies.EditarFaturamentoDaEmpresa)]
public class EditarFaturamentoDaEmpresaController(EditarFaturamentoDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Editar faturamento da empresa
    /// </summary>
    /// <remarks>
    /// Edita os dados de faturamento da empresa.
    /// </remarks>
    [HttpPut("office/empresas/{id}/faturamento")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarFaturamentoDaEmpresaIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarFaturamentoDaEmpresaIn>;
internal class ResponseExamples : ExamplesProvider<EditarFaturamentoDaEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    MetodoDePagamentoInvalido,
    EmpresaNaoEncontrada,
    ApenasMatrizesPodemSerFaturadas
>;
