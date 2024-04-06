namespace Syki.Back.Features.Cross.Login;

public class LoginRequestsExamples : IMultipleExamplesProvider<LoginIn>
{
    public IEnumerable<SwaggerExample<LoginIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new LoginIn
			{
				Email = "academico@syki.com",
				Password = "M1@Str0ngP4ssword#",
			}
		);
    }
}
