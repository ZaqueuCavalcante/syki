namespace Estud.Back.Features.Parents.GetParentStudentAgenda;

[ApiController, Authorize(Policies.GetParentStudentAgenda)]
public class GetParentStudentAgendaController(GetParentStudentAgendaService service) : ControllerBase
{
    /// <summary>
    /// Agenda do aluno vinculado
    /// </summary>
    /// <remarks>
    /// Retorna a agenda semanal de um aluno com vínculo ativo com o responsável logado.
    /// </remarks>
    [HttpGet("parents/students/{studentId}/agenda")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int studentId)
    {
        var result = await service.Get(studentId);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetParentStudentAgendaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    StudentNotFound
>;
