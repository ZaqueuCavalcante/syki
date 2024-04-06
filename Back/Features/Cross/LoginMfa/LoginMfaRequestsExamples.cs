namespace Syki.Back.Features.Cross.LoginMfa;

public class LoginMfaRequestsExamples : IMultipleExamplesProvider<LoginMfaIn>
{
    public IEnumerable<SwaggerExample<LoginMfaIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"TOTP Token",
			new LoginMfaIn
			{
				Token = "843972",
			}
		);
    }
}
