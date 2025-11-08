using Exato.Shared.Features.Office.BuscarEmpresa;

namespace Exato.Back.Features.Office.BuscarEmpresa;

[ApiController, Authorize(Policies.BuscarEmpresa)]
public class BuscarEmpresaController(BuscarEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Empresa
    /// </summary>
    /// <remarks>
    /// Retorna os dados da empresa.
    /// </remarks>
    [HttpGet("office/empresas/{id:int}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<EmpresaNaoEncontrada>;
