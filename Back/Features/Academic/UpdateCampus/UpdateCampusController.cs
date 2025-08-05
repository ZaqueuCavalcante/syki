namespace Syki.Back.Features.Academic.UpdateCampus;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class UpdateCampusController(UpdateCampusService service) : ControllerBase
{
    /// <summary>
    /// Editar campus
    /// </summary>
    /// <remarks>
    /// Edita os dados do campus informado.
    /// </remarks>
    [HttpPut("academic/campi")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateCampusIn data)
    {
        var result = await service.Update(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateCampusIn>;
internal class ResponseExamples : ExamplesProvider<CampusOut>;
internal class ErrorsExamples : ErrorExamplesProvider<CampusNotFound>;
