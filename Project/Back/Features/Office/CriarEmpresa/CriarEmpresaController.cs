using Exato.Shared.Features.Office.CriarEmpresa;

namespace Exato.Back.Features.Office.CriarEmpresa;

[ApiController, Authorize(Policies.CriarEmpresa)]
public class CriarEmpresaController(CriarEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Criar empresa
    /// </summary>
    /// <remarks>
    /// Cria uma nova empresa.
    /// </remarks>
    [HttpPost("office/empresas")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CriarEmpresaIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CriarEmpresaIn>;
internal class ResponseExamples : ExamplesProvider<CriarEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidCnpj,
    NomeDeEmpresaInvalido
>;
