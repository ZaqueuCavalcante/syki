using Exato.Shared.Features.Office.CreateCompany;

namespace Exato.Back.Features.Office.CreateCompany;

[ApiController, Authorize(Policies.CreateCompany)]
public class CreateCompanyController(CreateCompanyService service) : ControllerBase
{
    /// <summary>
    /// Criar company
    /// </summary>
    /// <remarks>
    /// Cria uma nova company no Exato Web.
    /// </remarks>
    [HttpPost("office/companies")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCompanyIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCompanyIn>;
internal class ResponseExamples : ExamplesProvider<CreateCompanyOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    EmpresaNaoEncontrada
>;
