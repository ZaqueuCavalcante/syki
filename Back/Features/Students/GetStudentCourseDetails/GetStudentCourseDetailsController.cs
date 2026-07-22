namespace Estud.Back.Features.Students.GetStudentCourseDetails;

[ApiController, Authorize(Policies.GetStudentCourseDetails)]
public class GetStudentCourseDetailsController(GetStudentCourseDetailsService service) : ControllerBase
{
    /// <summary>
    /// Detalhes do curso do aluno
    /// </summary>
    /// <remarks>
    /// Retorna os dados do curso atual do aluno logado, incluindo a grade curricular de disciplinas
    /// e o status de cada disciplina em relação ao aluno (não cursada, cursando, aprovada, dispensada ou reprovada).
    /// </remarks>
    [HttpGet("students/course")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get();
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetStudentCourseDetailsOut>;
internal class ErrorsExamples : ErrorExamplesProvider<StudentNotFound, StudentNotEnrolledInAnyCourse>;
