namespace Syki.Back.CreateEvaluationUnits;

[ApiController, AuthProfessor]
[EnableRateLimiting("Medium")]
public class CreateEvaluationUnitsController(CreateEvaluationUnitsService service) : ControllerBase
{
    [HttpPost("evaluation-units")]
    public async Task<IActionResult> Create([FromBody] CreateEvaluationUnitsIn data)
    {
        await service.Create(User.InstitutionId(), data);

        return Ok();
    }
}
