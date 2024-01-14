using Syki.Back.Services;
using Microsoft.AspNetCore.Mvc;

namespace Syki.Tests.Base;

[ApiController, Route("[controller]")]
public class TestsController : ControllerBase
{
    private readonly IAuthService _authService;
    public TestsController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("reset-password-token/{userId}")]
    public async Task<IActionResult> GetResetPasswordToken([FromRoute] Guid userId)
    {
        var result = await _authService.GetResetPasswordToken(userId);

        return Ok(result);
    }
}
