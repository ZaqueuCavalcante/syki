namespace Syki.Back.Features.Academic.CreateDiscipline;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateDisciplineController(CreateDisciplineService service) : ControllerBase
{
    /// <summary>
    /// Criar disciplina
    /// </summary>
    /// <remarks>
    /// Cria uma nova disciplina.
    /// </remarks>
    [HttpPost("academic/disciplines")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateDisciplineIn data)
    {
        var discipline = await service.Create(User.InstitutionId(), data);

        return Ok(discipline);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateDisciplineIn>
{
    public IEnumerable<SwaggerExample<CreateDisciplineIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Banco de Dados",
			new CreateDisciplineIn
			{
				Name = "Banco de Dados",
				Courses = [Guid.CreateVersion7(), Guid.CreateVersion7()]
			}
		);
        yield return SwaggerExample.Create(
			"Programação Orientada a Objetos",
			new CreateDisciplineIn
			{
				Name = "Programação Orientada a Objetos",
				Courses = [Guid.CreateVersion7()]
			}
		);
    }
}
