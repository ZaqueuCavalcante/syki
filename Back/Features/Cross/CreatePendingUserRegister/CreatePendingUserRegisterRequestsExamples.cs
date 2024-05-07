namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterRequestsExamples : IMultipleExamplesProvider<CreatePendingUserRegisterIn>
{
    public IEnumerable<SwaggerExample<CreatePendingUserRegisterIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new CreatePendingUserRegisterIn("academico@syki.com")
		);
        yield return SwaggerExample.Create(
			"Professor",
			new CreatePendingUserRegisterIn("professor@syki.com")
		);
        yield return SwaggerExample.Create(
			"Aluno",
			new CreatePendingUserRegisterIn("aluno@syki.com")
		);
    }
}
