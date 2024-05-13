namespace Syki.Back.Features.Academic.GetTeachers;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetTeachersController(GetTeachersService service) : ControllerBase
{
    [HttpGet("academic/teachers")]
    public async Task<IActionResult> Get()
    {
        var teachers = await service.Get(User.InstitutionId());

        return Ok(teachers);
    }
}
