namespace Syki.Back.Features.Cross.Login;

public class LoginErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new LoginWrongEmailOrPassword().ToSwaggerExampleErrorOut();
        yield return new LoginRequiresTwoFactor().ToSwaggerExampleErrorOut();
    }
}
