namespace Estud.Back.Features.Students.GetStudents;

[ApiController, Authorize(Policies.GetStudents)]
public class GetStudentsController(GetStudentsService service) : ControllerBase
{
    /// <summary>
    /// Alunos
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de alunos da instituição, ordenados por nome.
    /// </remarks>
    [HttpGet("students")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetStudentsIn query)
    {
        var students = await service.Get(query);
        return Ok(students);
    }
}

internal class RequestExamples : ExamplesProvider<GetStudentsIn>;
internal class ResponseExamples : ExamplesProvider<GetStudentsOut>;
