namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new InvalidEmail().ToSwaggerExampleErrorOut();
        yield return new EmailAlreadyUsed().ToSwaggerExampleErrorOut();
    }
}
