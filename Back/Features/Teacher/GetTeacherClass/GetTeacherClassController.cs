namespace Syki.Back.Features.Teacher.GetTeacherClass;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherClassController(GetTeacherClassService service) : ControllerBase
{
    [HttpGet("teacher/classes/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId(), User.Id(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
