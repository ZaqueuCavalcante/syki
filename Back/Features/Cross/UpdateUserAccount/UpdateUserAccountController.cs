namespace Syki.Back.Features.Cross.UpdateUserAccount;

[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class UpdateUserAccountController(UpdateUserAccountService service) : ControllerBase
{
    /// <summary>
    /// Atualiza conta
    /// </summary>
    /// <remarks>
    /// Atualiza a conta do usuário.
    /// </remarks>
    [HttpPut("user/account")]
    public async Task<IActionResult> Update([FromBody] UpdateUserAccountIn data)
    {
        var result = await service.Update(User.Id(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
