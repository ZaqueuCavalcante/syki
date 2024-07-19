namespace Syki.Back.Features.Cross.ResetPassword;

public class ResetPasswordErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new UserNotFound().ToSwaggerExampleErrorOut();
        yield return new InvalidResetToken().ToSwaggerExampleErrorOut();
        yield return new WeakPassword().ToSwaggerExampleErrorOut();
    }
}
