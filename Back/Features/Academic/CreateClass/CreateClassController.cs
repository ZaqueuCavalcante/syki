namespace Syki.Back.Features.Academic.CreateClass;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateClassController(CreateClassService service) : ControllerBase
{
    /// <summary>
    /// Criar turma
    /// </summary>
    /// <remarks>
    /// Cria uma nova turma.
    /// </remarks>
    [HttpPost("academic/classes")]
    [DbContextTransactionFilter]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateClassIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateClassIn>;
internal class ResponseExamples : ExamplesProvider<ClassOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    DisciplineNotFound,
    TeacherNotFound,
    AcademicPeriodNotFound,
    InvalidDay,
    InvalidHour,
    InvalidSchedule,
    ConflictingSchedules>;
