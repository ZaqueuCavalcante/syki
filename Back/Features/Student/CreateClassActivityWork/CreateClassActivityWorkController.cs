namespace Syki.Back.Features.Student.CreateClassActivityWork;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class CreateClassActivityWorkController(CreateClassActivityWorkService service) : ControllerBase
{
    /// <summary>
    /// Entregar atividade
    /// </summary>
    /// <remarks>
    /// Cria uma entrega para a atividade especificada
    /// </remarks>
    [HttpPost("student/activities/{id}/works")]
    [ProducesResponseType<ClassActivityWorkOut>(200)]
    [ProducesResponseType<SykiError>(400)]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateClassActivityWorkIn data)
    {
        var result = await service.Create(id, User.Id(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
