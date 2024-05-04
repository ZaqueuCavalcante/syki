namespace Syki.Back.CreateAcademicPeriod;

public class AcademicPeriodOutExamples : IMultipleExamplesProvider<AcademicPeriodOut>
{
    public IEnumerable<SwaggerExample<AcademicPeriodOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"2024.1",
			new AcademicPeriodOut
			{
				Id = "2024.1",
				Start = new DateOnly(2024, 02, 01),
				End = new DateOnly(2024, 06, 05),
			}
		);

        yield return SwaggerExample.Create(
			"2024.2",
			new AcademicPeriodOut
			{
				Id = "2024.2",
				Start = new DateOnly(2024, 07, 08),
				End = new DateOnly(2024, 12, 10),
			}
		);
    }
}
