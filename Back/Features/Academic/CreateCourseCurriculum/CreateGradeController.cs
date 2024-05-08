namespace Syki.Back.Features.Academic.CreateGrade;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateGradeController(CreateGradeService service) : ControllerBase
{
    [HttpPost("grades")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] GradeIn data)
    {
        var grade = await service.Create(User.InstitutionId(), data);

        return Ok(grade);
    }
}
