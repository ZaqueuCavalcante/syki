namespace Syki.Back.Features.Cross.Login;

public class LoginRequestsExamples : IMultipleExamplesProvider<LoginIn>
{
    public IEnumerable<SwaggerExample<LoginIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new LoginIn("academico@syki.com", "M1@Str0ngP4ssword#")
		);
        yield return SwaggerExample.Create(
			"Professor",
			new LoginIn("professor@syki.com", "M1@Str0ngP4ssword#")
		);
        yield return SwaggerExample.Create(
			"Aluno",
			new LoginIn("aluno@syki.com", "M1@Str0ngP4ssword#")
		);
    }
}
