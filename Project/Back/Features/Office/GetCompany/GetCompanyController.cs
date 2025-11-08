using Exato.Shared.Features.Office.GetCompany;

namespace Exato.Back.Features.Office.GetCompany;

[ApiController, Authorize(Policies.GetCompany)]
public class GetCompanyController(GetCompanyService service) : ControllerBase
{
    /// <summary>
    /// Company
    /// </summary>
    /// <remarks>
    /// Retorna a company.
    /// </remarks>
    [HttpGet("office/companies/{id:guid}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var company = await service.Get(id);
        return Ok(company);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCompanyOut>;
