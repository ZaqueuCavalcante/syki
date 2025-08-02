namespace Syki.Back.Features.Teacher.AddStudentClassActivityNote;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class AddStudentClassActivityNoteController(AddStudentClassActivityNoteService service) : ControllerBase
{
    /// <summary>
    /// Atribuir nota
    /// </summary>
    /// <remarks>
    /// Atribui a nota do aluno na atividade informada.
    /// </remarks>
    [HttpPost("teacher/class-activities/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] AddStudentClassActivityNoteIn data)
    {
        var result = await service.Add(User.Id(), id, data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : IMultipleExamplesProvider<AddStudentClassActivityNoteIn>
{
    public IEnumerable<SwaggerExample<AddStudentClassActivityNoteIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Nota do trabalho",
			new AddStudentClassActivityNoteIn(
				Guid.CreateVersion7(),
				5.48M
			)
		);
        yield return SwaggerExample.Create(
			"Nota da prova",
			new AddStudentClassActivityNoteIn(
				Guid.CreateVersion7(),
				10.00M
			)
		);
    }
}

public class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new ClassActivityNotFound().ToSwaggerExampleErrorOut();
        yield return new ClassNotFound().ToSwaggerExampleErrorOut();
        yield return new InvalidStudentClassNote().ToSwaggerExampleErrorOut();
    }
}
