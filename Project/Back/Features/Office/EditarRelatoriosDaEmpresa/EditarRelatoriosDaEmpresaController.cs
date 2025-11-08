using Exato.Shared.Features.Office.EditarRelatoriosDaEmpresa;

namespace Exato.Back.Features.Office.EditarRelatoriosDaEmpresa;

[ApiController, Authorize(Policies.EditarRelatoriosDaEmpresa)]
public class EditarRelatoriosDaEmpresaController(EditarRelatoriosDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Editar relatórios da empresa
    /// </summary>
    /// <remarks>
    /// Edita os relatórios da empresa.
    /// </remarks>
    [HttpPut("office/empresas/{id}/relatorios")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarRelatoriosDaEmpresaIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarRelatoriosDaEmpresaIn>;
internal class ResponseExamples : ExamplesProvider<EditarRelatoriosDaEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    RelatorioInvalido,
    EmpresaNaoEncontrada
>;
