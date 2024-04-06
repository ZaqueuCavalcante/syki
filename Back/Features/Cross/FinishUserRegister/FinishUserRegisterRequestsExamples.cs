namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterRequestsExamples : IMultipleExamplesProvider<FinishUserRegisterIn>
{
    public IEnumerable<SwaggerExample<FinishUserRegisterIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new FinishUserRegisterIn
			{
				Token = Guid.NewGuid().ToString(),
				Password = "M1@Str0ngP4ssword#"
			}
		);
    }
}
