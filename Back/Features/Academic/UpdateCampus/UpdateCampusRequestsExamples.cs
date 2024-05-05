namespace Syki.Back.Features.Academico.UpdateCampus;

public class UpdateCampusRequestsExamples : IMultipleExamplesProvider<UpdateCampusIn>
{
    public IEnumerable<SwaggerExample<UpdateCampusIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Agreste I",
			new UpdateCampusIn
			{
				Id = Guid.NewGuid(),
				Name = "Agreste I",
				City = "Caruaru - PE",
			}
		);
        yield return SwaggerExample.Create(
			"Suassuna I",
			new UpdateCampusIn
			{
				Id = Guid.NewGuid(),
				Name = "Suassuna I",
				City = "Recife - PE",
			}
		);
    }
}
