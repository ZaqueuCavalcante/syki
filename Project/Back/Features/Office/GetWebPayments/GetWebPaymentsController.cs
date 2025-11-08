using Exato.Shared.Features.Office.GetWebPayments;

namespace Exato.Back.Features.Office.GetWebPayments;

[ApiController, Authorize(Policies.GetWebPayments)]
public class GetWebPaymentsController(GetWebPaymentsService service) : ControllerBase
{
    /// <summary>
    /// Pagamentos
    /// </summary>
    /// <remarks>
    /// Retorna os pagamentos feitos no Exato Web.
    /// </remarks>
    [HttpGet("office/web-payments")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetWebPaymentsIn data)
    {
        var payments = await service.Get(data);
        return Ok(payments);
    }
}

internal class ResponseExamples : ExamplesProvider<GetWebPaymentsOut>;
