namespace Syki.Back.Features.Cross.SetupMfa;

public class SetupMfaErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return Throw.DE027.ToSwaggerExampleErrorOut();
    }
}
