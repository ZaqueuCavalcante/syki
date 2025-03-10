namespace Syki.Back.Features.Academic.CreateStudent;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateStudentController(CreateStudentService service) : ControllerBase
{
    /// <summary>
    /// Criar aluno
    /// </summary>
    /// <remarks>
    /// Cria um novo aluno.
    /// Um link para redefinição de senha será enviado pro email informado.
    /// </remarks>
    [HttpPost("academic/students")]
    [DbContextTransactionFilter]
    public async Task<IActionResult> Create([FromBody] CreateStudentIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
