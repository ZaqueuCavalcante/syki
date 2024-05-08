namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterRequestsExamples : IMultipleExamplesProvider<FinishUserRegisterIn>
{
    public IEnumerable<SwaggerExample<FinishUserRegisterIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new FinishUserRegisterIn(
				Guid.NewGuid().ToString(),
				"M1@Str0ngP4ssword#"
			)
		);
    }
}
