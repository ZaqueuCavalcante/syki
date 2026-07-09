namespace Estud.Back.Features.Students.GetStudents;

[ApiController, Authorize(Policies.GetStudents)]
public class GetStudentsController(GetStudentsService service) : ControllerBase
{
    /// <summary>
    /// Alunos
    /// </summary>
    /// <remarks>
    /// Retorna todos os alunos.
    /// </remarks>
    [HttpGet("students")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var students = await service.Get();
        return Ok(students);
    }
}

internal class ResponseExamples : ExamplesProvider<GetStudentsOut>;
