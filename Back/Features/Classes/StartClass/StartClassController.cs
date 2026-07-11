namespace Estud.Back.Features.Classes.StartClass;

[ApiController, Authorize(Policies.StartClass)]
public class StartClassController(StartClassService service) : ControllerBase
{
    /// <summary>
    /// Iniciar turma
    /// </summary>
    /// <remarks>
    /// Inicia a turma após o encerramento do período de matrícula, quando todas as matrículas já estão validadas.
    /// A turma deve estar em matrícula e o período de matrícula deve estar encerrado. Após iniciada, não é possível retroceder.
    /// </remarks>
    [HttpPut("classes/{id}/start")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Start(int id)
    {
        var result = await service.Start(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    ClassNotFound,
    ClassMustBeOnEnrollment,
    EnrollmentPeriodMustBeFinalized
>;
