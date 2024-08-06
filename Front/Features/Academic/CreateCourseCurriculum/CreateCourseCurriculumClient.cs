namespace Syki.Front.Features.Academic.CreateCourseCurriculum;

public class CreateCourseCurriculumClient(HttpClient http)
{
    public async Task<OneOf<CourseCurriculumOut, ErrorOut>> Create(string name, Guid courseId, List<CreateCourseCurriculumDisciplineIn> disciplines)
    {
        var data = new CreateCourseCurriculumIn
        {
            Name = name,
            CourseId = courseId,
            Disciplines = disciplines,
        };

        var response = await http.PostAsJsonAsync("/academic/course-curriculums", data);

        return await response.Resolve<CourseCurriculumOut>();
    }
}
