namespace Syki.Back.Features.Academic.CreateClass;

public class CreateClassRequestExamples : IMultipleExamplesProvider<CreateClassIn>
{
    public IEnumerable<SwaggerExample<CreateClassIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Banco de Dados",
			new CreateClassIn(
				Guid.NewGuid(),
				Guid.NewGuid(),
				"2024.1",
				40,
				[
					new(Day.Monday, Hour.H07_00, Hour.H10_00),
					new(Day.Thursday, Hour.H08_00, Hour.H10_30),
				]
			)
		);

        yield return SwaggerExample.Create(
			"Programação Orientada a Objetos",
			new CreateClassIn(
				Guid.NewGuid(),
				Guid.NewGuid(),
				"2024.2",
				40,
				[
					new(Day.Tuesday, Hour.H19_15, Hour.H22_00),
				]
			)
		);
    }
}
