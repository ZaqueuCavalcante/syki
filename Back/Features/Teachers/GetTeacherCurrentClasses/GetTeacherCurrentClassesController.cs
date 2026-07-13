namespace Estud.Back.Features.Teachers.GetTeacherCurrentClasses;

[ApiController, Authorize(Policies.GetTeacherCurrentClasses)]
public class GetTeacherCurrentClassesController(GetTeacherCurrentClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas atuais
    /// </summary>
    /// <remarks>
    /// Retorna as turmas que o professor está lecionando atualmente.
    /// </remarks>
    [HttpGet("teachers/current-classes")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get();
        return Ok(data);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherCurrentClassesOut>;
