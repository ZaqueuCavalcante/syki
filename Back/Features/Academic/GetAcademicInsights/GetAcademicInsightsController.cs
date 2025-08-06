namespace Syki.Back.Features.Academic.GetAcademicInsights;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetAcademicInsightsController(GetAcademicInsightsService service) : ControllerBase
{
    /// <summary>
    /// Insights
    /// </summary>
    /// <remarks>
    /// Retorna os Insights do Acadêmico.
    /// </remarks>
    [HttpGet("academic/insights")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get(User.InstitutionId);
        return Ok(insights);
    }
}

internal class ResponseExamples : ExamplesProvider<AcademicInsightsOut>;
