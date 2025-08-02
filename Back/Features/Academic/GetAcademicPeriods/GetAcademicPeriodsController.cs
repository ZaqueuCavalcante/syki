namespace Syki.Back.Features.Academic.GetAcademicPeriods;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetAcademicPeriods(GetAcademicPeriodsService service) : ControllerBase
{
    /// <summary>
    /// Períodos acadêmicos
    /// </summary>
    /// <remarks>
    /// Retorna todos os períodos acadêmicos.
    /// </remarks>
    [HttpGet("academic/academic-periods")]
    [ProducesResponseType(typeof(List<AcademicPeriodOut>), 200)]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());

        return Ok(periods);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<List<AcademicPeriodOut>>
{
    public IEnumerable<SwaggerExample<List<AcademicPeriodOut>>> GetExamples()
    {
        return
        [
            SwaggerExample.Create<List<AcademicPeriodOut>>(
				"Academic periods",
				[
					new AcademicPeriodOut
					{
						Id = "2024.1",
						StartAt = new DateOnly(2024, 02, 01),
						EndAt = new DateOnly(2024, 06, 05),
					},
					new AcademicPeriodOut
					{
						Id = "2024.2",
						StartAt = new DateOnly(2024, 07, 08),
						EndAt = new DateOnly(2024, 12, 10),
					}
				]
			)
		];
    }
}
