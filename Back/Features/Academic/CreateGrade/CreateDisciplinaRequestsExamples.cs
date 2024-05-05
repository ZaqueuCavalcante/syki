namespace Syki.Back.Features.Academico.CreateGrade;

public class CreateDisciplinaRequestsExamples : IMultipleExamplesProvider<GradeIn>
{
    public IEnumerable<SwaggerExample<GradeIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Grade ADS - 1.0",
			new GradeIn
			{
				Name = "Grade ADS - 1.0",
				CursoId = Guid.NewGuid(),
				Disciplinas = [new(Guid.NewGuid(), 1, 55, 70)]
			}
		);
    }
}
