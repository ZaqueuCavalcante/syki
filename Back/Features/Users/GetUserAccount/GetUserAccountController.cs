namespace Estud.Back.Features.Users.GetUserAccount;

[ApiController, Authorize(Policies.GetUserAccount)]
public class GetUserAccountController(GetUserAccountService service) : ControllerBase
{
    /// <summary>
    /// Conta do Usuário
    /// </summary>
    /// <remarks>
    /// Retorna dados da conta do usuário.
    /// </remarks>
    [HttpGet("users/account")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var account = await service.Get();
        return Ok(account);
    }
}

internal class ResponseExamples : ExamplesProvider<GetUserAccountOut>;
