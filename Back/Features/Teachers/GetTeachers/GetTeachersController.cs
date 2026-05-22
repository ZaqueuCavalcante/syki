namespace Syki.Back.Features.Teachers.GetTeachers;

[ApiController, Authorize(Policies.GetTeachers)]
public class GetTeachersController(GetTeachersService service) : ControllerBase
{
    /// <summary>
    /// Professores
    /// </summary>
    /// <remarks>
    /// Retorna todos os professores.
    /// </remarks>
    [HttpGet("teachers")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var teachers = await service.Get();
        return Ok(teachers);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeachersOut>;
