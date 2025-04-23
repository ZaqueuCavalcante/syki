namespace Syki.Back.Features.Teacher.CreateClassActivity;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateClassActivityController(CreateClassActivityService service) : ControllerBase
{
    /// <summary>
    /// Criar atividade
    /// </summary>
    /// <remarks>
    /// Cria uma atividade vinculada à turma especificada
    /// </remarks>
    [HttpPost("teacher/classes/{id}/activities")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateClassActivityIn data)
    {
        var result = await service.Create(User.Id(), id, data);

        return result.Match<IActionResult>(_ => Ok(), BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<CreateClassActivityIn>
{
    public IEnumerable<SwaggerExample<CreateClassActivityIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Atividade",
			new CreateClassActivityIn
			{
                Title = "Modelagem de Banco de Dados",
                Description = "Modele um banco de dados para um sistema de gerenciamento de biblioteca.",
                DueDate = DateTime.UtcNow.AddDays(7).ToDateOnly(),
                DueHour = Hour.H19_00,
			}
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new ClassNotFound().ToSwaggerExampleErrorOut();
        yield return new InvalidClassActivityWeight().ToSwaggerExampleErrorOut();
    }
}
