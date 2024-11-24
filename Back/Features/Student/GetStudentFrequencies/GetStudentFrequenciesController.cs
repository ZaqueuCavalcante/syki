namespace Syki.Back.Features.Student.GetStudentFrequencies;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentFrequenciesController(GetStudentFrequenciesService service) : ControllerBase
{
    /// <summary>
    /// Frequências
    /// </summary>
    /// <remarks>
    /// Retorna todas as frequências do aluno.
    /// </remarks>
    [HttpGet("student/frequencies")]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get(User.Id(), User.GetCourseCurriculumId());

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
