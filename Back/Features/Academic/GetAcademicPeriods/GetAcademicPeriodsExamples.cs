namespace Syki.Back.Features.Academic.GetAcademicPeriods;

public class GetAcademicPeriodsExamples : IMultipleExamplesProvider<List<AcademicPeriodOut>>
{
    public IEnumerable<SwaggerExample<List<AcademicPeriodOut>>> GetExamples()
    {
        return
        [
            SwaggerExample.Create<List<AcademicPeriodOut>>(
				"Academic periods",
				[
					new AcademicPeriodOut
					{
						Id = "2024.1",
						Start = new DateOnly(2024, 02, 01),
						End = new DateOnly(2024, 06, 05),
					},
					new AcademicPeriodOut
					{
						Id = "2024.2",
						Start = new DateOnly(2024, 07, 08),
						End = new DateOnly(2024, 12, 10),
					}
				]
			)
		];
    }
}
