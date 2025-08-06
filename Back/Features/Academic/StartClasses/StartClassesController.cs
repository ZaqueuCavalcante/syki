namespace Syki.Back.Features.Academic.StartClasses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class StartClassesController(StartClassesService service) : ControllerBase
{
    /// <summary>
    /// Iniciar turmas
    /// </summary>
    /// <remarks>
    /// Inicia várias turmas. <br/>
    /// Após isso, tanto professores quanto alunos passam a ter acesso às suas turmas.
    /// </remarks>
    [HttpPut("academic/classes/start")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Start([FromBody] StartClassesIn data)
    {
        var result = await service.Start(User.InstitutionId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<StartClassesIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidClassesList,
    ClassMustHaveOnEnrollmentStatus,
    EnrollmentPeriodNotFound,
    EnrollmentPeriodMustBeFinalized>;
