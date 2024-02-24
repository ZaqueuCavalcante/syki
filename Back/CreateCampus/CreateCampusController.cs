using Syki.Back.Extensions;
using Syki.Shared.CreateCampus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.CreateCampus;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateCampusController(CreateCampusService service) : ControllerBase
{
    [HttpPost("campi")]
    public async Task<IActionResult> Create([FromBody] CreateCampusIn data)
    {
        var campus = await service.Create(User.Facul(), data);

        return Ok(campus);
    }
}
