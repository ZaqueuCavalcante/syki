namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
		yield return Throw.DE019.ToSwaggerExampleErrorOut();
    }
}
