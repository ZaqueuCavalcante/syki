namespace Syki.Back.Features.Student.GetStudentClassFrequency;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentClassFrequencyController(GetStudentClassFrequencyService service) : ControllerBase
{
    /// <summary>
    /// Frequência total
    /// </summary>
    /// <remarks>
    /// Retorna a frequência total do aluno no curso.
    /// </remarks>
    [HttpGet("student/classes/{id}/frequency")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.Id(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
