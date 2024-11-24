namespace Syki.Back.Features.Cross.FinishUserRegister;

public class FinishUserRegisterResponseExamples : IMultipleExamplesProvider<UserOut>
{
    public IEnumerable<SwaggerExample<UserOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"User",
			new UserOut
			{
				Id = Guid.NewGuid(),
				Name = "Zaqueu Cavalcante",
				Email = "zaqueu.cavalcante@gmail.com",
				Password = "M1@Str0ngP4ssword#",
				InstitutionId = Guid.NewGuid(),
				Institution = "Universidade Federal Caruaruense",
				Role = UserRole.Student.ToString()
			}
		);
    }
}
