namespace Syki.Back.Features.Cross.SkipUserRegister;

public class SkipUserRegisterRequestsExamples : IExamplesProvider<SkipUserRegisterLoginIn>
{
    public SkipUserRegisterLoginIn GetExamples()
    {
		return new SkipUserRegisterLoginIn
		{
			UserId = Guid.NewGuid()
		};
    }
}
