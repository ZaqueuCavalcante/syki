using Syki.Shared.UpdateCampus;

namespace Syki.Back.UpdateCampus;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class UpdateCampusController(UpdateCampusService service) : ControllerBase
{
    [HttpPut("campi")]
    public async Task<IActionResult> Update([FromBody] UpdateCampusIn data)
    {
        var campus = await service.Update(User.InstitutionId(), data);

        return Ok(campus);
    }
}
