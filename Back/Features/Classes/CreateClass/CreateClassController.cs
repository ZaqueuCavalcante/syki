namespace Estud.Back.Features.Classes.CreateClass;

[ApiController, Authorize(Policies.CreateClass)]
public class CreateClassController(CreateClassService service) : ControllerBase
{
    /// <summary>
    /// Criar turma
    /// </summary>
    /// <remarks>
    /// Cria uma nova turma.
    /// </remarks>
    [HttpPost("classes")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateClassIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateClassIn>;
internal class ResponseExamples : ExamplesProvider<CreateClassOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CampusNotFound,
    DisciplineNotFound,
    AcademicPeriodNotFound,
    InvalidDay,
    InvalidHour,
    InvalidSchedule,
    ConflictingSchedules
>;
