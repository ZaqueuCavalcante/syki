namespace Syki.Back.Features.Teacher.GetTeacherClass;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherClassController(GetTeacherClassService service) : ControllerBase
{
    [HttpGet("teacher/classes/{classId}")]
    public async Task<IActionResult> Get([FromRoute] string classId)
    {
        var @class = await service.Get(User.InstitutionId(), User.Id(), classId);

        return Ok(@class);
    }
}
