namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new WeakPassword().ToSwaggerExampleErrorOut();
        yield return new InvalidRegistrationToken().ToSwaggerExampleErrorOut();
        yield return new UserAlreadyRegistered().ToSwaggerExampleErrorOut();
    }
}
