namespace Syki.Back.Features.Teacher.GetClassNotesRemainingWeights;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class GetClassNotesRemainingWeightsController(GetClassNotesRemainingWeightsService service) : ControllerBase
{
    /// <summary>
    /// Pesos restantes
    /// </summary>
    /// <remarks>
    /// Retorna os pesos restantes de cada nota da turma informada.
    /// </remarks>
    [HttpGet("teacher/classes/{id}/remaining-weights")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId, User.Id, id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
