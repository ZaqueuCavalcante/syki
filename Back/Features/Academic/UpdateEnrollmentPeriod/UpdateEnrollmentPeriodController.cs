namespace Syki.Back.Features.Academic.UpdateEnrollmentPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class UpdateEnrollmentPeriodController(UpdateEnrollmentPeriodService service) : ControllerBase
{
    [HttpPut("academic/enrollment-periods/{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateEnrollmentPeriodIn data)
    {
        var result = await service.Update(User.InstitutionId(), id, data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
