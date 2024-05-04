namespace Syki.Back.Features.Cross.GetMfaKey;

public class GetMfaKeyResponseExamples : IMultipleExamplesProvider<GetMfaKeyOut>
{
    public IEnumerable<SwaggerExample<GetMfaKeyOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Key",
			new GetMfaKeyOut
			{
				Key = "COZF2TE2BEWGHEB77A5THFYHPBC2KHPM"
			}
		);
    }
}
