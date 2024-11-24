namespace Syki.Back.Features.Academic.UpdateCampus;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class UpdateCampusController(UpdateCampusService service) : ControllerBase
{
    /// <summary>
    /// Editar campus
    /// </summary>
    /// <remarks>
    /// Edita os dados do campus informado.
    /// </remarks>
    [HttpPut("academic/campi")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(UpdateCampusErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateCampusIn data)
    {
        var result = await service.Update(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
