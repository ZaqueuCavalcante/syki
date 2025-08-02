namespace Syki.Back.Features.Academic.CreateTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateTeacherController(CreateTeacherService service) : ControllerBase
{
    /// <summary>
    /// Criar professor
    /// </summary>
    /// <remarks>
    /// Cria um novo professor.
    /// Um link para redefinição de senha será enviado pro email informado.
    /// </remarks>
    [HttpPost("academic/teachers")]
    [DbContextTransactionFilter]
    public async Task<IActionResult> Create([FromBody] CreateTeacherIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
