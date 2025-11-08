using Exato.Shared.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

namespace Exato.Back.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

[ApiController, Authorize(Policies.BuscarPotenciaisEmpresasDoUsuario)]
public class BuscarPotenciaisEmpresasDoUsuarioController(BuscarPotenciaisEmpresasDoUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Buscar potenciais empresas
    /// </summary>
    /// <remarks>
    /// Busca as potenciais empresas do usuário informado.
    /// </remarks>
    [HttpGet("office/usuarios/{id}/potenciais-empresas")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] BuscarPotenciaisEmpresasDoUsuarioIn data)
    {
        var empresas = await service.Get(id, data);
        return Ok(empresas);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarPotenciaisEmpresasDoUsuarioOut>;
