namespace Syki.Back.Features.Cross.ResetPassword;

public class ResetPasswordErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			Throw.DE019,
			new ErrorOut
			{
				Message = Throw.DE019
			}
		);
        yield return SwaggerExample.Create(
			Throw.DE020,
			new ErrorOut
			{
				Message = Throw.DE020
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
