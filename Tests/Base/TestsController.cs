using Syki.Shared;
using Syki.Back.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Syki.Tests.Base;

[ApiController, Route("[controller]")]
public class TestsController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public TestsController(SykiDbContext ctx) => _ctx = ctx;

    [HttpGet("reset-password-token/{userId}")]
    public async Task<IActionResult> GetResetPasswordToken([FromRoute] Guid userId)
    {
        var id = await _ctx.ResetPasswords
            .Where(r => r.UserId == userId && r.UsedAt == null)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        return Ok(new ResetPasswordTokenOut { Token = id == Guid.Empty ? null : id.ToString() });
    }

    [HttpGet("demo-setup-token/{email}")]
    public async Task<IActionResult> GetResetPasswordToken([FromRoute] string email)
    {
        var demo = await _ctx.Demos.FirstOrDefaultAsync(d => d.Email == email);

        return Ok(new DemoSetupTokenOut { Token = demo?.Id.ToString() });
    }
}
