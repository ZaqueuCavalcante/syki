namespace Estud.Back.Features.Users.UpdateUserAccount;

[ApiController, Authorize(Policies.UpdateUserAccount)]
public class UpdateUserAccountController(UpdateUserAccountService service) : ControllerBase
{
    /// <summary>
    /// Atualizar conta do usuário
    /// </summary>
    /// <remarks>
    /// Atualiza o nome do usuário logado.
    /// </remarks>
    [HttpPut("users/account")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateUserAccountIn data)
    {
        var result = await service.Update(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateUserAccountIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<InvalidUserName>;
