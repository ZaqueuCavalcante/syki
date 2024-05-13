namespace Syki.Back.Features.Academic.CreateDiscipline;

public class CreateDisciplineRequestsExamples : IMultipleExamplesProvider<CreateDisciplineIn>
{
    public IEnumerable<SwaggerExample<CreateDisciplineIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Banco de Dados",
			new CreateDisciplineIn
			{
				Name = "Banco de Dados",
				Courses = [Guid.NewGuid(), Guid.NewGuid()]
			}
		);
        yield return SwaggerExample.Create(
			"Programação Orientada a Objetos",
			new CreateDisciplineIn
			{
				Name = "Programação Orientada a Objetos",
				Courses = [Guid.NewGuid()]
			}
		);
    }
}
