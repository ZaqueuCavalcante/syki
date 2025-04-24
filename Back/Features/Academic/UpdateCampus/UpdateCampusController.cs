namespace Syki.Back.Features.Academic.UpdateCampus;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class UpdateCampusController(UpdateCampusService service) : ControllerBase
{
    /// <summary>
    /// Editar campus
    /// </summary>
    /// <remarks>
    /// Edita os dados do campus informado.
    /// </remarks>
    [HttpPut("academic/campi")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateCampusIn data)
    {
        var result = await service.Update(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

public class RequestExamples : IMultipleExamplesProvider<UpdateCampusIn>
{
    public IEnumerable<SwaggerExample<UpdateCampusIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Agreste I",
			new UpdateCampusIn
			{
				Id = Guid.NewGuid(),
				Name = "Agreste I",
                State = BrazilState.AP,
				City = "Caruaru",
			}
		);
        yield return SwaggerExample.Create(
			"Interior",
			new UpdateCampusIn
			{
				Id = Guid.NewGuid(),
				Name = "Interior",
                State = BrazilState.TO,
				City = "Palmas",
			}
		);
    }
}

public class ErrorExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new CampusNotFound().ToSwaggerExampleErrorOut();
    }
}
