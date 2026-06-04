namespace Syki.Back.Features.Teachers.GetTeacher;

[ApiController, Authorize(Policies.GetTeacher)]
public class GetTeacherController(GetTeacherService service) : ControllerBase
{
    /// <summary>
    /// Professor
    /// </summary>
    /// <remarks>
    /// Retorna os dados de um professor, incluindo os campus e disciplinas vinculados.
    /// </remarks>
    [HttpGet("teachers/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTeacherOut>;
internal class ErrorsExamples : ErrorExamplesProvider<TeacherNotFound>;
