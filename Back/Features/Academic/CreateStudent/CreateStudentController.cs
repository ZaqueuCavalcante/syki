namespace Syki.Back.Features.Academic.CreateStudent;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateStudentController(CreateStudentService service) : ControllerBase
{
    [HttpPost("academic/students")]
    public async Task<IActionResult> Create([FromBody] CreateStudentIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok,
            courseOfferingNotFound  => BadRequest(new ErrorOut { Message = courseOfferingNotFound.Message })
        );
    }
}
