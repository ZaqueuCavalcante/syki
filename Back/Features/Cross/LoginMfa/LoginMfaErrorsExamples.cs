namespace Syki.Back.Features.Cross.LoginMfa;

public class LoginMfaErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new LoginWrongEmailOrPassword().ToSwaggerExampleErrorOut();
        yield return new LoginRequiresTwoFactor().ToSwaggerExampleErrorOut();
    }
}
