namespace Syki.Back.Features.Cross.ResetPassword;

public class ResetPasswordRequestsExamples : IMultipleExamplesProvider<ResetPasswordIn>
{
    public IEnumerable<SwaggerExample<ResetPasswordIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new ResetPasswordIn
			{
				Token = Guid.NewGuid().ToString(),
				Password = "M1@Str0ngP4ssword#"
			}
		);
    }
}
