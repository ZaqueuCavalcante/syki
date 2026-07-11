namespace Estud.Back.Features.Students.AssignStudentToClass;

[ApiController, Authorize(Policies.AssignStudentToClass)]
public class AssignStudentToClassController(AssignStudentToClassService service) : ControllerBase
{
    /// <summary>
    /// Matricular aluno em turma
    /// </summary>
    /// <remarks>
    /// Aloca um aluno em uma turma manualmente pelo gestor.
    /// A turma deve estar em matrícula e possuir vagas disponíveis.
    /// </remarks>
    [HttpPost("students/{studentId:int}/classes")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Assign([FromRoute] int studentId, [FromBody] AssignStudentToClassIn data)
    {
        var result = await service.Assign(studentId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AssignStudentToClassIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    StudentNotFound,
    ClassNotFound,
    ClassMustBeOnEnrollment,
    NoVacanciesInClass,
    StudentAlreadyEnrolledInClass
>;
