namespace Syki.Back.Features.Academic.CreateTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateTeacherController(CreateTeacherService service) : ControllerBase
{
    [HttpPost("academic/teachers")]
    public async Task<IActionResult> Create([FromBody] CreateTeacherIn data)
    {
        var teacher = await service.Create(User.InstitutionId(), data);

        return Ok(teacher);
    }
}
