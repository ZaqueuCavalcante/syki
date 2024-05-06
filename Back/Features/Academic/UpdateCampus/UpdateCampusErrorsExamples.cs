namespace Syki.Back.Features.Academic.UpdateCampus;

public class UpdateCampusErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			Throw.DE010,
			new ErrorOut
			{
				Message = Throw.DE010
			}
		);
    }
}
