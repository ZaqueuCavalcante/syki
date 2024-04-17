namespace Syki.Back.CreateTurma;

public class TurmaInExamples : IMultipleExamplesProvider<TurmaIn>
{
    public IEnumerable<SwaggerExample<TurmaIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Banco de Dados",
			new TurmaIn(
				Guid.NewGuid(),
				Guid.NewGuid(),
				"2024.1",
				[
					new(Dia.Segunda, Hora.H07_00, Hora.H10_00),
					new(Dia.Quinta, Hora.H08_00, Hora.H10_30),
				]
			)
		);

        yield return SwaggerExample.Create(
			"Programação Orientada a Objetos",
			new TurmaIn(
				Guid.NewGuid(),
				Guid.NewGuid(),
				"2024.2",
				[
					new(Dia.Terca, Hora.H19_15, Hora.H22_00),
				]
			)
		);
    }
}
