namespace Syki.Back.Features.Cross.SetupMfa;

public class SetupMfaRequestsExamples : IMultipleExamplesProvider<SetupMfaIn>
{
    public IEnumerable<SwaggerExample<SetupMfaIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"TOTP Token",
			new SetupMfaIn
			{
				Token = "843972",
			}
		);
    }
}
