using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Syki.Shared.UpdateCampus;

namespace Syki.Back.UpdateCampus;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class UpdateCampusController(UpdateCampusService service) : ControllerBase
{
    [HttpPut("")]
    public async Task<IActionResult> Update([FromBody] UpdateCampusIn data)
    {
        var campus = await service.Update(User.Facul(), data);

        return Ok(campus);
    }
}
