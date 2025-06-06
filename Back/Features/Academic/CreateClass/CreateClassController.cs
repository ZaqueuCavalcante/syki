namespace Syki.Back.Features.Academic.CreateClass;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateClassController(CreateClassService service) : ControllerBase
{
    /// <summary>
    /// Criar turma
    /// </summary>
    /// <remarks>
    /// Cria uma nova turma.
    /// </remarks>
    [HttpPost("academic/classes")]
    [DbContextTransactionFilter]
    [ProducesResponseType(typeof(ClassOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> Create([FromBody] CreateClassIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateClassIn>
{
    public IEnumerable<SwaggerExample<CreateClassIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Banco de Dados",
			new CreateClassIn(
				Guid.CreateVersion7(),
				Guid.CreateVersion7(),
				"2024.1",
				40,
				[
					new(Day.Monday, Hour.H07_00, Hour.H10_00),
					new(Day.Thursday, Hour.H08_00, Hour.H10_30),
				]
			)
		);

        yield return SwaggerExample.Create(
			"Programação Orientada a Objetos",
			new CreateClassIn(
				Guid.CreateVersion7(),
				Guid.CreateVersion7(),
				"2024.2",
				40,
				[
					new(Day.Tuesday, Hour.H19_15, Hour.H22_00),
				]
			)
		);
    }
}

public class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new DisciplineNotFound().ToSwaggerExampleErrorOut();
        yield return new TeacherNotFound().ToSwaggerExampleErrorOut();
        yield return new AcademicPeriodNotFound().ToSwaggerExampleErrorOut();
        yield return new InvalidDay().ToSwaggerExampleErrorOut();
        yield return new InvalidHour().ToSwaggerExampleErrorOut();
        yield return new InvalidSchedule().ToSwaggerExampleErrorOut();
        yield return new ConflictingSchedules().ToSwaggerExampleErrorOut();
    }
}
