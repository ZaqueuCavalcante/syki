namespace Syki.Back.Features.Cross.LoginMfa;

public class LoginMfaRequestsExamples : IExamplesProvider<LoginMfaIn>
{
    public LoginMfaIn GetExamples()
    {
		return new LoginMfaIn
		{
			Token = "853941",
		};
    }
}
