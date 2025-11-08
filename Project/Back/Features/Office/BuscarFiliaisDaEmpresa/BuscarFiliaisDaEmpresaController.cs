using Exato.Shared.Features.Office.BuscarFiliaisDaEmpresa;

namespace Exato.Back.Features.Office.BuscarFiliaisDaEmpresa;

[ApiController, Authorize(Policies.BuscarFiliaisDaEmpresa)]
public class BuscarFiliaisDaEmpresaController(BuscarFiliaisDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Filiais da empresa
    /// </summary>
    /// <remarks>
    /// Retorna as filiais da empresa.
    /// </remarks>
    [HttpGet("office/empresas/{id:int}/filiais")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var filiais = await service.Get(id);
        return Ok(filiais);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarFiliaisDaEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<EmpresaNaoEncontrada>;
