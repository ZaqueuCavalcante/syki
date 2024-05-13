namespace Syki.Back.Features.Student.GetStudentDisciplines;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentDisciplinesController(GetStudentDisciplinesService service) : ControllerBase
{
    [HttpGet("student/disciplines")]
    public async Task<IActionResult> Get()
    {
        var disciplines = await service.Get(User.Id());

        return Ok(disciplines);
    }
}
