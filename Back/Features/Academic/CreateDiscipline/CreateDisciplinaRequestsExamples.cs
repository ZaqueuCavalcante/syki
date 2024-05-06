namespace Syki.Back.Features.Academic.CreateDisciplina;

public class CreateDisciplinaRequestsExamples : IMultipleExamplesProvider<DisciplinaIn>
{
    public IEnumerable<SwaggerExample<DisciplinaIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Banco de Dados",
			new DisciplinaIn
			{
				Name = "Banco de Dados",
				Cursos = [Guid.NewGuid(), Guid.NewGuid()]
			}
		);
        yield return SwaggerExample.Create(
			"Programação Orientada a Objetos",
			new DisciplinaIn
			{
				Name = "Programação Orientada a Objetos",
				Cursos = [Guid.NewGuid()]
			}
		);
    }
}
