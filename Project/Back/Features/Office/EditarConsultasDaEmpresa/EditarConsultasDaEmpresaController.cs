using Exato.Shared.Features.Office.EditarConsultasDaEmpresa;

namespace Exato.Back.Features.Office.EditarConsultasDaEmpresa;

[ApiController, Authorize(Policies.EditarConsultasDaEmpresa)]
public class EditarConsultasDaEmpresaController(EditarConsultasDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Editar consultas da empresa
    /// </summary>
    /// <remarks>
    /// Edita as configurações de consultas da empresa.
    /// </remarks>
    [HttpPut("office/empresas/{id}/consultas")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarConsultasDaEmpresaIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarConsultasDaEmpresaIn>;
internal class ResponseExamples : ExamplesProvider<EditarConsultasDaEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    EmpresaNaoEncontrada,
    LimiteDeConsultasSemanalInvalido,
    NivelDeAcessoADadosInvalido
>;
