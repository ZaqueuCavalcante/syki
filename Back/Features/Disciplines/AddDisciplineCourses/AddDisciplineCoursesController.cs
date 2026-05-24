namespace Syki.Back.Features.Disciplines.AddDisciplineCourses;

[ApiController, Authorize(Policies.AddDisciplineCourses)]
public class AddDisciplineCoursesController(AddDisciplineCoursesService service) : ControllerBase
{
    /// <summary>
    /// Vincular disciplina a cursos
    /// </summary>
    /// <remarks>
    /// Vincula uma disciplina existente a um ou mais cursos da instituição.
    /// </remarks>
    [HttpPost("disciplines/courses")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Add([FromBody] AddDisciplineCoursesIn data)
    {
        var result = await service.Add(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AddDisciplineCoursesIn>;
internal class ResponseExamples : ExamplesProvider<AddDisciplineCoursesOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    DisciplineNotFound,
    InvalidCoursesList
>;
