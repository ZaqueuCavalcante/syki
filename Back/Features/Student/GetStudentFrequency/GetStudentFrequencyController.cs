namespace Syki.Back.Features.Student.GetStudentFrequency;

/// <summary>
/// Retorna a Frequência total do Aluno no Curso.
/// </summary>
[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentFrequencyController(GetStudentFrequencyService service) : ControllerBase
{
    [HttpGet("student/frequency")]
    public async Task<IActionResult> Get()
    {
        var frequency = await service.Get(User.Id());

        return Ok(frequency);
    }
}
