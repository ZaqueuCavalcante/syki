using Exato.Shared.Features.Office.BuscarUsuariosDaEmpresa;

namespace Exato.Back.Features.Office.BuscarUsuariosDaEmpresa;

[ApiController, Authorize(Policies.BuscarUsuariosDaEmpresa)]
public class BuscarUsuariosDaEmpresaController(BuscarUsuariosDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Usuários da empresa
    /// </summary>
    /// <remarks>
    /// Retorna os usuários da empresa especificada.
    /// </remarks>
    [HttpGet("office/empresas/{id:int}/usuarios")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] BuscarUsuariosDaEmpresaIn data)
    {
        var usuarios = await service.Get(id, data);
        return Ok(usuarios);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarUsuariosDaEmpresaOut>;
