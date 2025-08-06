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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var account = await service.Get(User.Id);
        return Ok(account);
    }
}

internal class ResponseExamples : ExamplesProvider<GetUserAccountOut>;
