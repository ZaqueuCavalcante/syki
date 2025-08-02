namespace Syki.Back.Features.Academic.UpdateEnrollmentPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class UpdateEnrollmentPeriodController(UpdateEnrollmentPeriodService service) : ControllerBase
{
    /// <summary>
    /// Editar período de matrícula
    /// </summary>
    /// <remarks>
    /// Edita os dados do período de matrícula informado.
    /// </remarks>
    [HttpPut("academic/enrollment-periods/{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateEnrollmentPeriodIn data)
    {
        var result = await service.Update(User.InstitutionId(), id, data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
