namespace Estud.Back.Features.Students.GetStudentDetails;

[ApiController, Authorize(Policies.GetStudentDetails)]
public class GetStudentDetailsController(GetStudentDetailsService service) : ControllerBase
{
    /// <summary>
    /// Buscar detalhes do aluno
    /// </summary>
    /// <remarks>
    /// Retorna os detalhes de um aluno da instituição do usuário logado, incluindo a oferta de curso atual e as turmas em que está matriculado.
    /// </remarks>
    [HttpGet("students/{studentId:int}/details")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int studentId)
    {
        var result = await service.Get(studentId);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<GetStudentDetailsOut>;
internal class ResponseExamples : ExamplesProvider<GetStudentDetailsOut>;
internal class ErrorsExamples : ErrorExamplesProvider<StudentNotFound>;
