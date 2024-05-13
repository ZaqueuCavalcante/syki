namespace Syki.Back.Features.Academic.CreateStudent;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateAlunoController(CreateStudentService service) : ControllerBase
{
    [HttpPost("academic/students")]
    public async Task<IActionResult> Create([FromBody] CreateStudentIn data)
    {
        var aluno = await service.Create(User.InstitutionId(), data);

        return Ok(aluno);
    }
}
