namespace Syki.Back.Features.Academic.CreateCampus;

public class CreateCampusRequestsExamples : IMultipleExamplesProvider<CreateCampusIn>
{
    public IEnumerable<SwaggerExample<CreateCampusIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Agreste I",
			new CreateCampusIn
			{
				Name = "Agreste I",
				City = "Caruaru - PE",
			}
		);
        yield return SwaggerExample.Create(
			"Suassuna I",
			new CreateCampusIn
			{
				Name = "Suassuna I",
				City = "Recife - PE",
			}
		);
    }
}
