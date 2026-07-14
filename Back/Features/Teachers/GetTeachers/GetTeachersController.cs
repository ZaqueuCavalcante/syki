namespace Estud.Back.Features.Teachers.GetTeachers;

[ApiController, Authorize(Policies.GetTeachers)]
public class GetTeachersController(GetTeachersService service) : ControllerBase
{
    /// <summary>
    /// Professores
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de professores da instituição, ordenados por nome.
    /// </remarks>
    [HttpGet("teachers")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetTeachersIn query)
    {
        var teachers = await service.Get(query);
        return Ok(teachers);
    }
}

internal class RequestExamples : ExamplesProvider<GetTeachersIn>;
internal class ResponseExamples : ExamplesProvider<GetTeachersOut>;
