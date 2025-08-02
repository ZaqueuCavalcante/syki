namespace Syki.Back.Features.Academic.CreateClassroom;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateClassroomController(CreateClassroomService service) : ControllerBase
{
    /// <summary>
    /// Criar sala de aula
    /// </summary>
    /// <remarks>
    /// Cria uma nova sala de aula.
    /// </remarks>
    [HttpPost("academic/classrooms")]
    [ProducesResponseType(typeof(CreateClassroomOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> Create([FromBody] CreateClassroomIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateClassroomIn>
{
    public IEnumerable<SwaggerExample<CreateClassroomIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Sala 05",
			new CreateClassroomIn
            {
                Name = "Sala 05",
                Capacity = 40,
                CampusId = Guid.CreateVersion7(),
			}
		);
        yield return SwaggerExample.Create(
			"Laboratório de Química",
			new CreateClassroomIn
            {
                Name = "Laboratório de Química",
                Capacity = 35,
                CampusId = Guid.CreateVersion7(),
			}
		);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<CreateClassroomOut>
{
    public IEnumerable<SwaggerExample<CreateClassroomOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Sala 05",
            new CreateClassroomOut
            {
                Id = Guid.CreateVersion7(),
                Name = "Sala 05",
            }
        );
    }
}

internal class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new CampusNotFound().ToSwaggerExampleErrorOut();
    }
}
