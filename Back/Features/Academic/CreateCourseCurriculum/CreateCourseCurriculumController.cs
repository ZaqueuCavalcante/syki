namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCourseCurriculumController(CreateCourseCurriculumService service) : ControllerBase
{
    /// <summary>
    /// Criar grade curricular
    /// </summary>
    /// <remarks>
    /// Cria uma nova grade curricular.
    /// </remarks>
    [HttpPost("academic/course-curriculums")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateCourseCurriculumIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateCourseCurriculumIn>
{
    public IEnumerable<SwaggerExample<CreateCourseCurriculumIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Grade ADS - 1.0",
			new CreateCourseCurriculumIn
			{
				Name = "Grade ADS - 1.0",
				CourseId = Guid.CreateVersion7(),
				Disciplines = [new(Guid.CreateVersion7(), 1, 55, 70)]
			}
		);
    }
}
