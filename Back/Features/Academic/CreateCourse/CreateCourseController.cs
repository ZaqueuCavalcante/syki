namespace Syki.Back.Features.Academic.CreateCourse;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCourseController(CreateCourseService service) : ControllerBase
{
    /// <summary>
    /// Criar curso
    /// </summary>
    /// <remarks>
    /// Cria um novo curso.
    /// </remarks>
    [HttpPost("academic/courses")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateCourseIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateCourseIn>
{
    public IEnumerable<SwaggerExample<CreateCourseIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"ADS",
			new CreateCourseIn
			{
				Name = "Análise e Desenvolvimento de Sistemas",
                Type = CourseType.Tecnologo,
                Disciplines = ["Programação Orientada a Objetos", "Banco de Dados"],
			}
		);
        yield return SwaggerExample.Create(
			"Direito",
			new CreateCourseIn
			{
				Name = "Direito",
                Type = CourseType.Bacharelado,
                Disciplines = ["Direito Civil", "Direito Penal"],
			}
		);
    }
}
