namespace Syki.Back.Features.Academic.GetTeachers;

/// <summary>
/// Retorna todos os Professores.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeachersController(GetTeachersService service) : ControllerBase
{
    [HttpGet("academic/teachers")]
    public async Task<IActionResult> Get()
    {
        var teachers = await service.Get(User.InstitutionId());

        return Ok(teachers);
    }
}
