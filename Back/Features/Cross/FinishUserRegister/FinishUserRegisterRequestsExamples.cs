namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterRequestsExamples : IExamplesProvider<FinishUserRegisterIn>
{
	public FinishUserRegisterIn GetExamples()
	{
		return new FinishUserRegisterIn(
			Guid.NewGuid().ToString(),
			"M1@Str0ngP4ssword#"
		);
	}
}
