namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return Throw.DE016.ToSwaggerExampleErrorOut();
        yield return Throw.DE017.ToSwaggerExampleErrorOut();
    }
}
