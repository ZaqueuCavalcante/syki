namespace Exato.Back.Features.Web.AccountManagementCompany;

[ApiController, Authorize(Policies.GetUserAccount)]
public class AccountManagementCompanyController() : ControllerBase
{
    /// <summary>
    /// GetCompanyUser
    /// </summary>
    [HttpGet("web/[action]/{userUid}")]
    public async Task<IActionResult> GetCompanyUser([FromRoute] Guid userUid, [FromServices] GetCompanyUserService service)
    {
        var user = await service.Get(userUid);
        return Ok(user);
    }
}
