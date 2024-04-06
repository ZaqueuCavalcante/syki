namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			Throw.DE024,
			new ErrorOut
			{
				Message = Throw.DE024
			}
		);
        yield return SwaggerExample.Create(
			Throw.DE025,
			new ErrorOut
			{
				Message = Throw.DE025
			}
		);
        yield return SwaggerExample.Create(
			Throw.DE015,
			new ErrorOut
			{
				Message = Throw.DE015
			}
		);
    }
}
