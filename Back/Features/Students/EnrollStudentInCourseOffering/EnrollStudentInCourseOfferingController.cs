namespace Estud.Back.Features.Students.EnrollStudentInCourseOffering;

[ApiController, Authorize(Policies.EnrollStudentInCourseOffering)]
public class EnrollStudentInCourseOfferingController(EnrollStudentInCourseOfferingService service) : ControllerBase
{
    /// <summary>Matricular aluno em oferta de curso</summary>
    /// <remarks>Vincula um aluno a uma oferta de curso, criando uma matrícula.</remarks>
    [HttpPost("students/{studentId:int}/course-offerings")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Enroll([FromRoute] int studentId, [FromBody] EnrollStudentInCourseOfferingIn data)
    {
        var result = await service.Enroll(studentId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EnrollStudentInCourseOfferingIn>;
internal class ResponseExamples : ExamplesProvider<EnrollStudentInCourseOfferingOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    StudentNotFound,
    CourseOfferingNotFound,
    StudentAlreadyEnrolledInCourseOffering
>;
