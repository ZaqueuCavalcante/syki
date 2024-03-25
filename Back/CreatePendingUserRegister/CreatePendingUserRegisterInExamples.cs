using Swashbuckle.AspNetCore.Filters;

namespace Syki.Back.CreatePendingUserRegister;

public class CreatePendingUserRegisterInExamples : IMultipleExamplesProvider<CreatePendingUserRegisterIn>
{
    public IEnumerable<SwaggerExample<CreatePendingUserRegisterIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Acadêmico",
			new CreatePendingUserRegisterIn
			{
				Email = "academico@syki.com"
			}
		);
        yield return SwaggerExample.Create(
			"Professor",
			new CreatePendingUserRegisterIn
			{
				Email = "professor@syki.com"
			}
		);
        yield return SwaggerExample.Create(
			"Aluno",
			new CreatePendingUserRegisterIn
			{
				Email = "aluno@syki.com"
			}
		);
    }
}
