namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return Throw.DE015.ToSwaggerExampleErrorOut();
        yield return Throw.DE024.ToSwaggerExampleErrorOut();
        yield return Throw.DE025.ToSwaggerExampleErrorOut();
    }
}
