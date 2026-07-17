namespace Estud.Back.Features.Disciplines.GetDisciplineTeachers;

[ApiController, Authorize(Policies.GetDisciplineTeachers)]
public class GetDisciplineTeachersController(GetDisciplineTeachersService service) : ControllerBase
{
    /// <summary>
    /// Professores aptos a lecionar a disciplina
    /// </summary>
    /// <remarks>
    /// Retorna os professores vinculados à disciplina, ordenados por nome.
    /// </remarks>
    [HttpGet("disciplines/{id}/teachers")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetDisciplineTeachersOut>;
internal class ErrorsExamples : ErrorExamplesProvider<DisciplineNotFound>;
