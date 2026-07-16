namespace Estud.Back.Features.Parents.GetParentStudents;

[ApiController, Authorize(Policies.GetParentStudents)]
public class GetParentStudentsController(GetParentStudentsService service) : ControllerBase
{
    /// <summary>
    /// Alunos vinculados ao responsável
    /// </summary>
    /// <remarks>
    /// Retorna a lista de alunos com vínculo ativo com o responsável logado, ordenados por nome.
    /// </remarks>
    [HttpGet("parents/students")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var students = await service.Get();
        return Ok(students);
    }
}

internal class ResponseExamples : ExamplesProvider<GetParentStudentsOut>;
