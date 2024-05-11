namespace Syki.Back.Features.Academic.UpdateCampus;

/// <summary>
/// Atualiza os dados de um campus.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class UpdateCampusController(UpdateCampusService service) : ControllerBase
{
    [HttpPut("academic/campi")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(UpdateCampusErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateCampusIn data)
    {
        var campus = await service.Update(User.InstitutionId(), data);

        return Ok(campus);
    }
}
