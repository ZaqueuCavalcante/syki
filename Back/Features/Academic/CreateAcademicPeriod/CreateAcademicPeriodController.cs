namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateAcademicPeriodController(CreateAcademicPeriodService service) : ControllerBase
{
    /// <summary>
    /// Criar período acadêmico
    /// </summary>
    /// <remarks>
    /// Cria um novo período acadêmico.
    /// </remarks>
    [HttpPost("academic/academic-periods")]
    [ProducesResponseType(typeof(AcademicPeriodOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateAcademicPeriodIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateAcademicPeriodIn>
{
    public IEnumerable<SwaggerExample<CreateAcademicPeriodIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"2024.1",
			new CreateAcademicPeriodIn
			{
				Id = "2024.1",
				StartAt = new DateOnly(2024, 02, 01),
				EndAt = new DateOnly(2024, 06, 05),
			}
		);

        yield return SwaggerExample.Create(
			"2024.2",
			new CreateAcademicPeriodIn
			{
				Id = "2024.2",
				StartAt = new DateOnly(2024, 07, 08),
				EndAt = new DateOnly(2024, 12, 10),
			}
		);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<AcademicPeriodOut>
{
    public IEnumerable<SwaggerExample<AcademicPeriodOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"2024.1",
			new AcademicPeriodOut
			{
				Id = "2024.1",
				StartAt = new DateOnly(2024, 02, 01),
				EndAt = new DateOnly(2024, 06, 05),
			}
		);

        yield return SwaggerExample.Create(
			"2024.2",
			new AcademicPeriodOut
			{
				Id = "2024.2",
				StartAt = new DateOnly(2024, 07, 08),
				EndAt = new DateOnly(2024, 12, 10),
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidAcademicPeriod().ToSwaggerExampleErrorOut();
        yield return new InvalidAcademicPeriodDates().ToSwaggerExampleErrorOut();
        yield return new AcademicPeriodAlreadyExists().ToSwaggerExampleErrorOut();
        yield return new InvalidAcademicPeriodEndDate().ToSwaggerExampleErrorOut();
        yield return new InvalidAcademicPeriodStartDate().ToSwaggerExampleErrorOut();
    }
}
