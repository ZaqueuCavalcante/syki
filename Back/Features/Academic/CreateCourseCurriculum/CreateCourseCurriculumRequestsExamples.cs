namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

public class CreateCourseCurriculumRequestsExamples : IMultipleExamplesProvider<CreateCourseCurriculumIn>
{
    public IEnumerable<SwaggerExample<CreateCourseCurriculumIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Grade ADS - 1.0",
			new CreateCourseCurriculumIn
			{
				Name = "Grade ADS - 1.0",
				CourseId = Guid.NewGuid(),
				Disciplines = [new(Guid.NewGuid(), 1, 55, 70)]
			}
		);
    }
}
