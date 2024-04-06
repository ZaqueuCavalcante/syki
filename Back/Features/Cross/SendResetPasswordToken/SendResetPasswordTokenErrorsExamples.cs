namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenErrorsExamples : IMultipleExamplesProvider<ErrorOut>
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
    }
}
