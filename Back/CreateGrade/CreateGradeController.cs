namespace Syki.Back.CreateGrade;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateGradeController(CreateGradeService service) : ControllerBase
{
    [HttpPost("grades")]
    public async Task<IActionResult> Create([FromBody] GradeIn data)
    {
        var grade = await service.Create(User.InstitutionId(), data);

        return Ok(grade);
    }
}
