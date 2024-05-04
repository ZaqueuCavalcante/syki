namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			Throw.DE017,
			new ErrorOut
			{
				Message = Throw.DE017
			}
		);
        yield return SwaggerExample.Create(
			Throw.DE016,
			new ErrorOut
			{
				Message = Throw.DE016
			}
		);
    }
}
