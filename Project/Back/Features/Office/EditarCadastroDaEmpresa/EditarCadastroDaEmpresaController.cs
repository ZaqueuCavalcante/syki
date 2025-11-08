using Exato.Shared.Features.Office.EditarCadastroDaEmpresa;

namespace Exato.Back.Features.Office.EditarCadastroDaEmpresa;

[ApiController, Authorize(Policies.EditarCadastroDaEmpresa)]
public class EditarCadastroDaEmpresaController(EditarCadastroDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Editar cadastro da empresa
    /// </summary>
    /// <remarks>
    /// Edita os dados de cadastro da empresa.
    /// </remarks>
    [HttpPut("office/empresas/{id}/cadastro")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarCadastroDaEmpresaIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarCadastroDaEmpresaIn>;
internal class ResponseExamples : ExamplesProvider<EditarCadastroDaEmpresaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidCnpj,
    NomeDeEmpresaInvalido,
    EmpresaNaoEncontrada,
    MatrizInvalida,
    ApenasMatrizesPodemPossuirVendedorResponsavel
>;
