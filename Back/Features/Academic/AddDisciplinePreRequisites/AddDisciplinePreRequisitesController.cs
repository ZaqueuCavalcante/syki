namespace Syki.Back.Features.Academic.AddDisciplinePreRequisites;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class AddDisciplinePreRequisitesController(AddDisciplinePreRequisitesService service) : ControllerBase
{
    /// <summary>
    /// Adicionar pré-requisitos à uma disciplina
    /// </summary>
    /// <remarks>
    /// Adiciona pré-requisitos à uma disciplina, dentro de uma grade curricular.
    /// </remarks>
    [HttpPost("academic/course-curriculums/{courseCurriculumId}/{disciplineId}/pre-requisites")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Add([FromRoute] Guid courseCurriculumId, [FromRoute] Guid disciplineId, [FromBody] AddDisciplinePreRequisitesIn data)
    {
        var result = await service.Add(User.InstitutionId(), courseCurriculumId, disciplineId, data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<AddDisciplinePreRequisitesIn>
{
    public IEnumerable<SwaggerExample<AddDisciplinePreRequisitesIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Pré-Requisitos",
			new AddDisciplinePreRequisitesIn
			{
				PreRequisites = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()]
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new CourseCurriculumNotFound().ToSwaggerExampleErrorOut();
        yield return new DisciplineNotFound().ToSwaggerExampleErrorOut();
        yield return new InvalidDisciplinesList().ToSwaggerExampleErrorOut();
    }
}
