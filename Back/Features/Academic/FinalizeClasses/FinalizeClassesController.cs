namespace Syki.Back.Features.Academic.FinalizeClasses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class FinalizeClassesController(FinalizeClassesService service) : ControllerBase
{
    /// <summary>
    /// Finalizar turmas
    /// </summary>
    /// <remarks>
    /// Finaliza várias turmas.
    /// </remarks>
    [HttpPut("academic/classes/finalize")]
    public async Task<IActionResult> Finalize([FromBody] FinalizeClassesIn data)
    {
        var result = await service.Finalize(User.InstitutionId(), data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
