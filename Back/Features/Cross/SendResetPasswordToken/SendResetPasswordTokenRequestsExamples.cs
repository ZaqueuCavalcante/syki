namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenRequestsExamples : IMultipleExamplesProvider<SendResetPasswordTokenIn>
{
    public IEnumerable<SwaggerExample<SendResetPasswordTokenIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new SendResetPasswordTokenIn("academico@syki.com")
		);
        yield return SwaggerExample.Create(
			"Professor",
			new SendResetPasswordTokenIn("professor@syki.com")
		);
        yield return SwaggerExample.Create(
			"Aluno",
			new SendResetPasswordTokenIn("aluno@syki.com")
		);
    }
}
