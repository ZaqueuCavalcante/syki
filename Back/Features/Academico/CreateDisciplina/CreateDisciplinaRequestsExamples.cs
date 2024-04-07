namespace Syki.Back.Features.Academico.CreateDisciplina;

public class CreateDisciplinaRequestsExamples : IMultipleExamplesProvider<DisciplinaIn>
{
    public IEnumerable<SwaggerExample<DisciplinaIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Banco de Dados",
			new DisciplinaIn
			{
				Code = "BDD",
				Nome = "Banco de Dados",
				Cursos = [Guid.NewGuid(), Guid.NewGuid()]
			}
		);
        yield return SwaggerExample.Create(
			"Programação Orientada a Objetos",
			new DisciplinaIn
			{
				Code = "POO",
				Nome = "Programação Orientada a Objetos",
				Cursos = [Guid.NewGuid()]
			}
		);
    }
}
