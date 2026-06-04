namespace Syki.Back.Features.Students.GetStudent;

[ApiController, Authorize(Policies.GetStudent)]
public class GetStudentController(GetStudentService service) : ControllerBase
{
    /// <summary>Buscar aluno</summary>
    /// <remarks>Retorna os dados de um aluno, incluindo a oferta de curso atual.</remarks>
    [HttpGet("students/{studentId:int}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get([FromRoute] int studentId)
    {
        var result = await service.Get(studentId);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<GetStudentOut>;
internal class ResponseExamples : ExamplesProvider<GetStudentOut>;
internal class ErrorsExamples : ErrorExamplesProvider<StudentNotFound>;
