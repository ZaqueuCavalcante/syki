namespace Syki.Back.Features.Cross.ResetPassword;

public class ResetPasswordErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
		yield return Throw.DE019.ToSwaggerExampleErrorOut();
		yield return Throw.DE020.ToSwaggerExampleErrorOut();
		yield return Throw.DE015.ToSwaggerExampleErrorOut();
    }
}
