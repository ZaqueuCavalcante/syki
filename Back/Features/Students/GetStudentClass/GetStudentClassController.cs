namespace Estud.Back.Features.Students.GetStudentClass;

[ApiController, Authorize(Policies.GetStudentClass)]
public class GetStudentClassController(GetStudentClassService service) : ControllerBase
{
    /// <summary>
    /// Buscar turma do aluno
    /// </summary>
    /// <remarks>
    /// Retorna os detalhes de uma turma em que o aluno logado está matriculado,
    /// incluindo horários e o status da sua matrícula.
    /// </remarks>
    [HttpGet("students/classes/{id:int}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetStudentClassOut>;
internal class ErrorsExamples : ErrorExamplesProvider<ClassNotFound, StudentNotEnrolledInClass>;
