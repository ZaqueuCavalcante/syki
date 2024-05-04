namespace Syki.Back.CreateAcademicPeriod;

public class CreateAcademicPeriodInExamples : IMultipleExamplesProvider<CreateAcademicPeriodIn>
{
    public IEnumerable<SwaggerExample<CreateAcademicPeriodIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"2024.1",
			new CreateAcademicPeriodIn
			{
				Id = "2024.1",
				Start = new DateOnly(2024, 02, 01),
				End = new DateOnly(2024, 06, 05),
			}
		);

        yield return SwaggerExample.Create(
			"2024.2",
			new CreateAcademicPeriodIn
			{
				Id = "2024.2",
				Start = new DateOnly(2024, 07, 08),
				End = new DateOnly(2024, 12, 10),
			}
		);
    }
}
