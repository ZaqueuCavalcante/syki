namespace Syki.Back.Features.Academic.FinalizeClasses;

/// <summary>
/// Finaliza várias Turmas.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class FinalizeClassesController(FinalizeClassesService service) : ControllerBase
{
    [HttpPut("academic/classes/finalize")]
    public async Task<IActionResult> Finalize([FromBody] FinalizeClassesIn data)
    {
        var result = await service.Finalize(User.InstitutionId(), data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
