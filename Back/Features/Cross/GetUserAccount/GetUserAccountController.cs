namespace Syki.Back.Features.Cross.GetUserAccount;

[ApiController, AuthBearer]
[EnableRateLimiting("Medium")]
public class GetUserAccountController(GetUserAccountService service) : ControllerBase
{
    /// <summary>
    /// Conta
    /// </summary>
    /// <remarks>
    /// Retorna dados da conta do usuário.
    /// </remarks>
    [HttpGet("user/account")]
    [ProducesResponseType(typeof(GetUserAccountOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var account = await service.Get(User.Id());
        
        return Ok(account);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<GetUserAccountOut>
{
	public IEnumerable<SwaggerExample<GetUserAccountOut>> GetExamples()
	{
		yield return SwaggerExample.Create(
			"Edson Gomes",
			new GetUserAccountOut()
			{
				Id = Guid.CreateVersion7(),
				Name = "Edson Gomes",
				Email = "edson.gomes@syki.com.br",
				Institution = "UFPE",
				Role = UserRole.Student,
			}
		);
        yield return SwaggerExample.Create(
			"Maria Júlia",
			new GetUserAccountOut()
			{
				Id = Guid.CreateVersion7(),
				Name = "Maria Júlia",
				Email = "maria.julia@syki.com.br",
				Institution = "Faculdade Nova Roma",
				Role = UserRole.Teacher,
			}
		);
    }
}
