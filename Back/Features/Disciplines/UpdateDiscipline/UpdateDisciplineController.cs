namespace Estud.Back.Features.Disciplines.UpdateDiscipline;

[ApiController, Authorize(Policies.UpdateDiscipline)]
public class UpdateDisciplineController(UpdateDisciplineService service) : ControllerBase
{
    /// <summary>
    /// Editar disciplina
    /// </summary>
    /// <remarks>
    /// Edita o nome da disciplina informada.
    /// </remarks>
    [HttpPut("disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateDisciplineIn data)
    {
        var result = await service.Update(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateDisciplineIn>;
internal class ResponseExamples : ExamplesProvider<UpdateDisciplineOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidDisciplineName,
    DisciplineNotFound
>;
