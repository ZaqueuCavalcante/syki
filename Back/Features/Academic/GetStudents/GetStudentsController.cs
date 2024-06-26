namespace Syki.Back.Features.Academic.GetStudents;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentsController(GetStudentsService service) : ControllerBase
{
    [HttpGet("academic/students")]
    public async Task<IActionResult> Get()
    {
        var students = await service.Get(User.InstitutionId());
        
        return Ok(students);
    }
}
