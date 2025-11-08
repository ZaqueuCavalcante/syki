using Exato.Shared.Features.Office.UpdateCompany;

namespace Exato.Back.Features.Office.UpdateCompany;

[ApiController, Authorize(Policies.UpdateCompany)]
public class UpdateCompanyController(UpdateCompanyService service) : ControllerBase
{
    /// <summary>
    /// Editar company no Exato Web
    /// </summary>
    /// <remarks>
    /// Edita os dados de uma company no Exato Web.
    /// </remarks>
    [HttpPut("office/companies/{id:guid}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCompanyIn data)
    {
        var result = await service.Update(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateCompanyIn>;
internal class ResponseExamples : ExamplesProvider<UpdateCompanyOut>;
internal class ErrorsExamples : ErrorExamplesProvider<WebCompanyNotFound>;
