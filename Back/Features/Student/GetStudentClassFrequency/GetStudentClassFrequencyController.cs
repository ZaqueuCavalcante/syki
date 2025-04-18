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
    [HttpGet("student/classes/{classId}/frequency")]
    public async Task<IActionResult> Get([FromRoute] Guid classId)
    {
        var result = await service.Get(User.Id(), classId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
