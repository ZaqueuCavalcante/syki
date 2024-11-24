namespace Syki.Back.Features.Student.GetStudentFrequency;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentFrequencyController(GetStudentFrequencyService service) : ControllerBase
{
    /// <summary>
    /// Frequência total
    /// </summary>
    /// <remarks>
    /// Retorna a frequência total do aluno no curso.
    /// </remarks>
    [HttpGet("student/frequency")]
    public async Task<IActionResult> Get()
    {
        var frequency = await service.Get(User.Id());

        return Ok(frequency);
    }
}
